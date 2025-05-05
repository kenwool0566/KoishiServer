using Google.Protobuf;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    // TODO: add lineup manager
    public class Session
    {
        private readonly NetworkStream _stream;

        public Session(NetworkStream stream)
        {
            _stream = stream;
        }

        public async Task Send<C, M>(C commandType, M message)
            where C : Enum
            where M : IMessage
        {
            ushort commandId = Convert.ToUInt16(commandType);
            byte[] body = message.ToByteArray();
            Packet packet = Packet.Headless(commandId, body);
            byte[] data = packet.ToByteArray();
            await _stream.WriteAsync(data, 0, data.Length);
        }
    }
}
