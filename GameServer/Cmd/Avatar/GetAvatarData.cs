using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class AvatarHandler
    {
        public static async Task CmdGetAvatarDataCsReq(Session session, Packet packet)
        {
            Avatar march = CreateMaxAvatar(1001);
            Avatar yunli = CreateMaxAvatar(1221);
            Avatar trailblazer = CreateMaxAvatar(8001);

            GetAvatarDataScRsp rsp = new GetAvatarDataScRsp
            {
                AvatarList = { march, yunli, trailblazer },
                IsGetAll = true,
            };

            await session.Send(CmdAvatarType.CmdGetAvatarDataScRsp, rsp);
        }

        private static Avatar CreateMaxAvatar(uint avatarId)
        {
            return new Avatar
            {
                Rank = 6,
                Promotion = 6,
                Level = 80,
                BaseAvatarId = avatarId,
            };
        }
    }
}
