using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdPlayerGetTokenCsReq(Session session, Packet packet)
        {
            PlayerGetTokenScRsp rsp = new PlayerGetTokenScRsp
            {
                Uid = 1,
                Msg = "OK",
            };

            await session.Send(CmdPlayerType.CmdPlayerGetTokenScRsp, rsp);
        }
    }
}
