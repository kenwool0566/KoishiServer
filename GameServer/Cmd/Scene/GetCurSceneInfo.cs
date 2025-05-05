using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class SceneHandler
    {
        public static async Task CmdGetCurSceneInfoCsReq(Session session, Packet packet)
        {
            SceneEntityInfo playerEntity = new SceneEntityInfo
            {
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -550,
                        Y = 19364,
                        Z = 4480,
                    },
                    Rot = new Vector(),
                },
                Actor = new SceneActorInfo
                {
                    AvatarType = AvatarType.AvatarFormalType,
                    BaseAvatarId = 1221,
                    MapLayer = 2,
                    Uid = 1,
                },
            };

            SceneEntityGroupInfo entityGroup = new SceneEntityGroupInfo
            {
                State = 1,
                EntityList = { playerEntity },
            };

            GetCurSceneInfoScRsp rsp = new GetCurSceneInfoScRsp
            {
                Scene = new SceneInfo
                {
                    PlaneId = 20101,
                    EntryId = 20101 * 100 + 1,
                    FloorId = 20101 * 1000 + 1,
                    GameModeType = 2,
                    EntityGroupList = { entityGroup },
                },
            };

            await session.Send(CmdSceneType.CmdGetCurSceneInfoScRsp, rsp);
        }
    }
}
