using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdGetMultiPathAvatarInfoCsReq(Session session, Packet packet)
        {
            GetMultiPathAvatarInfoScRsp rsp = new GetMultiPathAvatarInfoScRsp();
            rsp.CurAvatarPath.Add(1001, session.Persistent!.GetTrailblazerType());
            rsp.CurAvatarPath.Add(8001, session.Persistent!.GetMarchType());

            await session.Send(CmdPlayerType.CmdGetMultiPathAvatarInfoScRsp, rsp);
        }
    }
}
