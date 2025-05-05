using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class SceneHandler
    {
        public static async Task CmdGetSceneMapInfoCsReq(Session session, Packet packet)
        {
            GetSceneMapInfoCsReq req;
            try { req = GetSceneMapInfoCsReq.Parser.ParseFrom(packet.BodyData); }
            catch { req = new GetSceneMapInfoCsReq(); }

            List<SceneMapInfo> mapInfos = req.FloorIdList
                .Select(floorId => new SceneMapInfo
                {
                    EntryId = (floorId - 1) / 1000,
                    CurMapEntryId = (floorId + 9) / 10,
                    FloorId = floorId,
                })
                .ToList();

            GetSceneMapInfoScRsp rsp = new GetSceneMapInfoScRsp
            {
                IGFIKGHLLNO = req.IGFIKGHLLNO,
                EntryStoryLineId = req.EntryStoryLineId,
                ContentId = req.ContentId,
                SceneMapInfo = { mapInfos },
            };

            await session.Send(CmdSceneType.CmdGetSceneMapInfoScRsp, rsp);
        }
    }
}
