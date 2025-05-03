using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.Common.Util;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public class ItemHandler
    {
        public static async Task HandleGetBag(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdItemType.CmdGetBagScRsp;
            GetBagScRsp rsp = new GetBagScRsp();
            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }
    }
}
