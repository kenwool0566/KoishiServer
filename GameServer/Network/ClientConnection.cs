using Serilog;
using System;
using System.Buffers;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public partial class ClientConnection
    {
        private Socket _socket;
        private SocketAsyncEventArgs _receiveArgs;
        private byte[] _buffer;
        private readonly MemoryStream _packetBuffer = new();

        private const int MIN_PACKET_SIZE = 16;
        private const int HEADER_PREFIX_SIZE = 12;

        public ClientConnection(Socket socket)
        {
            _socket = socket;
            _buffer = ArrayPool<byte>.Shared.Rent(2048);
            _receiveArgs = new SocketAsyncEventArgs();
            _receiveArgs.SetBuffer(_buffer, 0, _buffer.Length);
            _receiveArgs.Completed += ReceiveCompleted;
            StartReceive();
        }

        private void StartReceive()
        {
            if (!_socket.ReceiveAsync(_receiveArgs)) ProcessReceive(_receiveArgs);
        }

        private void ReceiveCompleted(object? sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                Close();
                return;
            }

            if (e.Buffer == null)
            {
                StartReceive();
                return;
            }

            _packetBuffer.Write(e.Buffer, e.Offset, e.BytesTransferred);
            TryExtractPackets();
            StartReceive();
        }

        private void TryExtractPackets()
        {
            byte[] bufferData = _packetBuffer.ToArray();
            int position = 0;

            while (position <= bufferData.Length - MIN_PACKET_SIZE)
            {
                int headerPos = FindMagic(bufferData, Packet.HEAD_MAGIC, position);
                if (headerPos == -1) break;

                if (headerPos + HEADER_PREFIX_SIZE > bufferData.Length)
                {
                    position = headerPos;
                    break;
                }

                ushort cmdId = (ushort)((bufferData[headerPos + 4] << 8) | bufferData[headerPos + 5]);
                ushort headSize = (ushort)((bufferData[headerPos + 6] << 8) | bufferData[headerPos + 7]);
                uint bodySize = (uint)((bufferData[headerPos + 8] << 24) |
                                      (bufferData[headerPos + 9] << 16) |
                                      (bufferData[headerPos + 10] << 8) |
                                       bufferData[headerPos + 11]);

                int totalSize = HEADER_PREFIX_SIZE + headSize + (int)bodySize + Packet.TAIL_MAGIC.Length;

                if (headerPos + totalSize > bufferData.Length)
                {
                    position = headerPos;
                    break;
                }

                int tailPos = headerPos + totalSize - Packet.TAIL_MAGIC.Length;

                bool validTail = (
                    bufferData[tailPos + 0] == Packet.TAIL_MAGIC[0]
                    && bufferData[tailPos + 1] == Packet.TAIL_MAGIC[1]
                    && bufferData[tailPos + 2] == Packet.TAIL_MAGIC[2]
                    && bufferData[tailPos + 3] == Packet.TAIL_MAGIC[3]
                );

                if (!validTail)
                {
                    position = headerPos + Packet.HEAD_MAGIC.Length;
                    continue;
                }

                Packet packet = new Packet
                {
                    CommandId = cmdId,
                    HeaderSize = headSize,
                    BodySize = bodySize
                };

                if (headSize > 0)
                {
                    packet.HeaderData = new byte[headSize];
                    Array.Copy(bufferData, headerPos + HEADER_PREFIX_SIZE, packet.HeaderData, 0, headSize);
                }

                if (bodySize > 0)
                {
                    packet.BodyData = new byte[bodySize];
                    Array.Copy(bufferData, headerPos + HEADER_PREFIX_SIZE + headSize, packet.BodyData, 0, (int)bodySize);
                }


                HandlePacket(packet);
                position = tailPos + Packet.TAIL_MAGIC.Length;
            }

            if (position > 0)
            {
                int remainingLength = bufferData.Length - position;
                if (remainingLength > 0)
                {
                    _packetBuffer.SetLength(0);
                    _packetBuffer.Write(bufferData, position, remainingLength);
                }
                else
                {
                    _packetBuffer.SetLength(0);
                }
            }
        }

        private int FindMagic(byte[] data, byte[] magic, int startPos)
        {
            for (int i = startPos; i <= data.Length - magic.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < magic.Length; j++)
                {
                    if (data[i + j] != magic[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found) return i;
            }

            return -1;
        }

        private void Close()
        {
            _socket.Close();
            ArrayPool<byte>.Shared.Return(_buffer);
        }

        public async Task SendPacket(Packet packet)
        {
            ushort headerSize = (ushort)(packet.HeaderData?.Length ?? 0);
            uint bodySize = (uint)(packet.BodyData?.Length ?? 0);

            int totalSize = Packet.HEAD_MAGIC.Length + 2 + 2 + 4 + headerSize + (int)bodySize + Packet.TAIL_MAGIC.Length;
            byte[] data = new byte[totalSize];
            int position = 0;

            Array.Copy(Packet.HEAD_MAGIC, 0, data, position, Packet.HEAD_MAGIC.Length);
            position += Packet.HEAD_MAGIC.Length;

            data[position++] = (byte)(packet.CommandId >> 8);
            data[position++] = (byte)(packet.CommandId & 0xFF);

            data[position++] = (byte)(headerSize >> 8);
            data[position++] = (byte)(headerSize & 0xFF);

            data[position++] = (byte)((bodySize >> 24) & 0xFF);
            data[position++] = (byte)((bodySize >> 16) & 0xFF);
            data[position++] = (byte)((bodySize >> 8) & 0xFF);
            data[position++] = (byte)(bodySize & 0xFF);

            if (headerSize > 0)
            {
                Array.Copy(packet.HeaderData!, 0, data, position, (int)headerSize);
                position += (int)headerSize;
            }

            if (bodySize > 0)
            {
                Array.Copy(packet.BodyData!, 0, data, position, (int)bodySize);
                position += (int)bodySize;
            }

            Array.Copy(Packet.TAIL_MAGIC, 0, data, position, Packet.TAIL_MAGIC.Length);

            await _socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        }
    }
}
