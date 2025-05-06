using KoishiServer.Common.Config;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class SceneHandler
    {
        public static async Task CmdGetCurSceneInfoCsReq(Session session, Packet packet)
        {
            Persistent persistent = session.Persistent!;

            uint leaderAvatarId = persistent.Lineup
                .Select(x => x.Value)
                .Where(v => v != null && v.Leader)
                .Select(v => v!.Id)
                .FirstOrDefault();

            SceneEntityInfo playerEntity = new SceneEntityInfo
            {
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = persistent.Position.X,
                        Y = persistent.Position.Y,
                        Z = persistent.Position.Z,
                    },
                    Rot = new Vector
                    {
                        X = persistent.Rotation.X,
                        Y = persistent.Rotation.Y,
                        Z = persistent.Rotation.Z,
                    },
                },
                Actor = new SceneActorInfo
                {
                    AvatarType = AvatarType.AvatarFormalType,
                    BaseAvatarId = leaderAvatarId,
                    MapLayer = persistent.MapLayer,
                    Uid = 1,
                },
            };

            SceneEntityGroupInfo entityGroup = new SceneEntityGroupInfo
            {
                State = 1,
                EntityList = { playerEntity },
            };

            SceneInfo scene = new SceneInfo
            {
                PlaneId = persistent.Scene.PlaneId,
                FloorId = persistent.Scene.FloorId,
                EntryId = persistent.Scene.EntryId,
                GameModeType = 2,
                EntityGroupList = { entityGroup },
            };

            GetCurSceneInfoScRsp rsp = new GetCurSceneInfoScRsp
            {
                Scene = scene
            };

            await session.Send(CmdSceneType.CmdGetCurSceneInfoScRsp, rsp);
        }
    }
}
