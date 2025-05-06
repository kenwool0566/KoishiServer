using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class LineupHandler
    {
        public static async Task CmdQuitLineupCsReq(Session session, Packet packet)
        {
            await session.Send(CmdLineupType.CmdQuitLineupScRsp, new QuitLineupScRsp());
        }
    }
}
