using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdPlayerLoginFinishCsReq(Session session, Packet packet)
        {
            await session.Send(CmdPlayerType.CmdPlayerLoginFinishScRsp, new PlayerLoginFinishScRsp());
        }
    }
}
