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

            Avatar march = NewMaxAvatar(1001);
            Avatar yunli = NewMaxAvatar(1221);
            Avatar trailblazer = NewMaxAvatar(8001);

            GetAvatarDataScRsp rsp = new GetAvatarDataScRsp
            {
                AvatarList = { march, yunli, trailblazer },
                IsGetAll = true,
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        private static Avatar NewMaxAvatar(uint avatarId)
        {
            Avatar avatar = new Avatar
            {
                Rank = 6,
                Promotion = 6,
                Level = 80,
                Exp = 0,
                BaseAvatarId = avatarId,
            };

            return avatar;
        }
    }
}
