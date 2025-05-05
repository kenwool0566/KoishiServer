using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class SceneHandler
    {
        public static async Task CmdSceneEntityMoveCsReq(Session session, Packet packet)
        {
            await session.Send(CmdSceneType.CmdSceneEntityMoveScRsp, new SceneEntityMoveScRsp());
        }
    }
}
