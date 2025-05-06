using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class SceneHandler
    {
        public static async Task CmdSceneEntityMoveCsReq(Session session, Packet packet)
        {
            SceneEntityMoveCsReq req;
            try { req = SceneEntityMoveCsReq.Parser.ParseFrom(packet.BodyData); }
            catch { req = new SceneEntityMoveCsReq(); }

            EntityMotion? playerEntity = req.EntityMotionList.FirstOrDefault(m => m.EntityId == 0);
            MotionInfo motion = playerEntity!.Motion;

            session.Persistent!.SetMapLayer(playerEntity!.MapLayer);
            session.Persistent!.SetPosRot(
                (motion.Pos.X, motion.Pos.Y, motion.Pos.Z),
                (motion.Rot.X, motion.Rot.Y, motion.Rot.Z)
            );

            await session.Send(CmdSceneType.CmdSceneEntityMoveScRsp, new SceneEntityMoveScRsp());
        }
    }
}
