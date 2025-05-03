using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public class SceneHandler
    {
        public static async Task HandleGetCurSceneInfo(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdSceneType.CmdGetCurSceneInfoScRsp;

            Vector playerRot = new Vector();
            Vector playerPos = new Vector
            {
                X = -550,
                Y = 19364,
                Z = 4480,
            };

            MotionInfo playerMotion = new MotionInfo
            {
                Pos = playerPos,
                Rot = playerRot,
            };

            SceneActorInfo playerActor = new SceneActorInfo
            {
                AvatarType = AvatarType.AvatarFormalType,
                BaseAvatarId = 1221,
                MapLayer = 2,
                Uid = 1,
            };

            SceneEntityInfo playerEntity = new SceneEntityInfo
            {
                Motion = playerMotion,
                Actor = playerActor,
                // EntityCaseCase = SceneEntityInfo.EntityCaseOneofCase.Actor,
            };

            SceneEntityGroupInfo groupInfo = new SceneEntityGroupInfo
            {
                State = 1,
                EntityList = { playerEntity },
            };

            SceneInfo scene = new SceneInfo
            {
                PlaneId = 20101,
                EntryId = 20101 * 100 + 1,
                FloorId = 20101 * 1000 + 1,
                GameModeType = 2,
                EntityGroupList = { groupInfo },
            };

            GetCurSceneInfoScRsp rsp = new GetCurSceneInfoScRsp
            {
                Scene = scene,
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandleGetSceneMapInfo(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdSceneType.CmdGetSceneMapInfoScRsp;

            GetSceneMapInfoCsReq req;
            try { req = GetSceneMapInfoCsReq.Parser.ParseFrom(inPacket.BodyData); }
            catch { req = new GetSceneMapInfoCsReq(); }

            List<SceneMapInfo> mapInfos = req.FloorIdList
                .Select(floorId => new SceneMapInfo
                {
                    EntryId = floorId,
                    CurMapEntryId = floorId,
                    FloorId = floorId,
                })
                .ToList();

            GetSceneMapInfoScRsp rsp = new GetSceneMapInfoScRsp
            {
                IGFIKGHLLNO = req.IGFIKGHLLNO,
                SceneMapInfo = { mapInfos },
                EntryStoryLineId = req.EntryStoryLineId,
                ContentId = req.ContentId,
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }
    }
}
