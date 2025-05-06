using Google.Protobuf;
using KoishiServer.Common.Config;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public partial class Session
    {
        private readonly NetworkStream _stream;
        public SRToolData? SRToolsData;
        public Persistent? Persistent;

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

    public partial class Session
    {
        public async Task<bool> LoadSRTools()
        {
            bool fileHasChanged = SRToolsConfigLoader.HasFileChanged();
            if (fileHasChanged) SRToolsData = await SRToolsConfigLoader.LoadConfigAsync();
            return fileHasChanged;
        }

        public async Task LoadPersistent()
        {
            Persistent = await PersistentLoader.LoadConfigAsync();
        }
        
        public async Task SavePersistent()
        {
            if (Persistent != null) await PersistentLoader.SaveToFileAsync(Persistent);
        }
    }
}
