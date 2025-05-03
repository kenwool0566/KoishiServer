using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public class AvatarHandler
    {
        public static async Task HandleGetAvatarData(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdAvatarType.CmdGetAvatarDataScRsp;

            Avatar yunli = new Avatar
            {
                Rank = 6,
                Promotion = 6,
                Level = 80,
                Exp = 0,
                BaseAvatarId = 1221,
            };

            GetAvatarDataScRsp rsp = new GetAvatarDataScRsp
            {
                AvatarList = { yunli },
                IsGetAll = true,
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }
    }
}
