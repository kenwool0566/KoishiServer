using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class MissionHandler
    {
        public static async Task CmdGetMissionStatusCsReq(Session session, Packet packet)
        {
            GetMissionStatusCsReq req;
            try { req = GetMissionStatusCsReq.Parser.ParseFrom(packet.BodyData); }
            catch { req = new GetMissionStatusCsReq(); }

            List<Mission> subMissions = req.SubMissionIdList
                .Select(id => new Mission
                {
                    Id = id,
                    Progress = 1,
                    Status = MissionStatus.MissionFinish
                })
                .ToList();

            GetMissionStatusScRsp rsp = new GetMissionStatusScRsp
            {
                FinishedMainMissionIdList = { req.MainMissionIdList },
                SubMissionStatusList = { subMissions }
            };

            await session.Send(CmdMissionType.CmdGetMissionStatusScRsp, rsp);
        }
    }
}
