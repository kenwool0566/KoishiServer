using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdGetBasicInfoCsReq(Session session, Packet packet)
        {
            GetBasicInfoScRsp rsp = new GetBasicInfoScRsp
            {
                PlayerSettingInfo = new PlayerSettingInfo(),
                IsGenderSet = true,
                Gender = (uint)session.Persistent!.GetTrailblazerGender(),
            };

            await session.Send(CmdPlayerType.CmdGetBasicInfoScRsp, rsp);
        }
    }
}
