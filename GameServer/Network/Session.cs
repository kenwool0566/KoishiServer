using Google.Protobuf;
using KoishiServer.Common.Config;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    // TODO: add lineup manager
    public class Session
    {
        private readonly NetworkStream _stream;
        public SRToolData? SRToolsData;

        public Session(NetworkStream stream)
        {
            _stream = stream;
        }

        public async Task<bool> LoadSRTools()
        {
            string filePath = SRToolData.SRToolsConfigFilePath;
            bool fileHasChanged = ConfigLoader.HasFileChanged(filePath);
            if (fileHasChanged) SRToolsData = await ConfigLoader.FromFileAsync<SRToolData>(filePath);
            return fileHasChanged;
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
