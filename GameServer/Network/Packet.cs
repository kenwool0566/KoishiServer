using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public class Packet
    {
        public ushort CommandId { get; set; }
        public byte[] HeadData { get; set; } = Array.Empty<byte>();
        public byte[] BodyData { get; set; } = Array.Empty<byte>();

        private static readonly byte[] HEAD_MAGIC = new byte[] { 0x9D, 0x74, 0xC7, 0x14 };
        private static readonly byte[] TAIL_MAGIC = new byte[] { 0xD7, 0xA1, 0x52, 0xC8 };

        public static Packet Headless(ushort commandId, byte[] bodyData)
        {
            return new Packet
            {
                CommandId = commandId,
                BodyData = bodyData
            };
        }

        public byte[] ToByteArray()
        {
            ushort headLength = (ushort)(HeadData?.Length ?? 0);
            uint bodyLength = (uint)(BodyData?.Length ?? 0);

            int totalLength = HEAD_MAGIC.Length + 2 + 2 + 4 + (int)headLength + (int)bodyLength + TAIL_MAGIC.Length;
            byte[] result = new byte[totalLength];
            int pos = 0;

            Array.Copy(HEAD_MAGIC, 0, result, pos, HEAD_MAGIC.Length);
            pos += HEAD_MAGIC.Length;

            result[pos++] = (byte)(CommandId >> 8);
            result[pos++] = (byte)(CommandId & 0xFF);

            result[pos++] = (byte)(headLength >> 8);
            result[pos++] = (byte)(headLength & 0xFF);

            result[pos++] = (byte)((bodyLength >> 24) & 0xFF);
            result[pos++] = (byte)((bodyLength >> 16) & 0xFF);
            result[pos++] = (byte)((bodyLength >> 8) & 0xFF);
            result[pos++] = (byte)(bodyLength & 0xFF);

            if (headLength > 0)
            {
                Array.Copy(HeadData!, 0, result, pos, headLength);
                pos += headLength;
            }

            if (bodyLength > 0)
            {
                Array.Copy(BodyData!, 0, result, pos, (int)bodyLength);
                pos += (int)bodyLength;
            }

            Array.Copy(TAIL_MAGIC, 0, result, pos, TAIL_MAGIC.Length);

            return result;
        }

        public static async Task<Packet> ReadAsync(NetworkStream stream)
        {
            byte[] headMagic = new byte[4];
            await ReadExact(stream, headMagic);
            if (!IsHeadMagic(headMagic)) throw new InvalidDataException("Invalid HEAD_MAGIC");

            ushort cmdId = await ReadUInt16Async(stream);
            ushort headLength = await ReadUInt16Async(stream);
            uint bodyLength = await ReadUInt32Async(stream);

            byte[] head = new byte[headLength];
            await ReadExact(stream, head);

            byte[] body = new byte[bodyLength];
            await ReadExact(stream, body);

            byte[] tailMagic = new byte[4];
            await ReadExact(stream, tailMagic);
            if (!IsTailMagic(tailMagic)) throw new InvalidDataException("Invalid TAIL_MAGIC");

            return new Packet
            {
                CommandId = cmdId,
                HeadData = head,
                BodyData = body
            };
        }

        private static async Task ReadExact(NetworkStream stream, byte[] buffer)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                int read = await stream.ReadAsync(buffer.AsMemory(offset, buffer.Length - offset));
                if (read == 0) throw new IOException("Connection closed");
                offset += read;
            }
        }

        private static async Task<ushort> ReadUInt16Async(NetworkStream stream)
        {
            byte[] buffer = new byte[2];
            await ReadExact(stream, buffer);
            return (ushort)((buffer[0] << 8) | buffer[1]);
        }

        private static async Task<uint> ReadUInt32Async(NetworkStream stream)
        {
            byte[] buffer = new byte[4];
            await ReadExact(stream, buffer);
            return ((uint)buffer[0] << 24) | ((uint)buffer[1] << 16) | ((uint)buffer[2] << 8) | buffer[3];
        }

        private static bool IsHeadMagic(byte[] headMagic)
        {
            return (headMagic[0] == HEAD_MAGIC[0] &&
                    headMagic[1] == HEAD_MAGIC[1] &&
                    headMagic[2] == HEAD_MAGIC[2] &&
                    headMagic[3] == HEAD_MAGIC[3]);
        }

        private static bool IsTailMagic(byte[] tailMagic)
        {
            return (tailMagic[0] == TAIL_MAGIC[0] &&
                    tailMagic[1] == TAIL_MAGIC[1] &&
                    tailMagic[2] == TAIL_MAGIC[2] &&
                    tailMagic[3] == TAIL_MAGIC[3]);
        }
    }
}
