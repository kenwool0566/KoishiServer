using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class SceneHandler
    {
        public static async Task CmdSceneCastSkillCostMpCsReq(Session session, Packet packet)
        {
            SceneCastSkillCostMpCsReq req;
            try { req = SceneCastSkillCostMpCsReq.Parser.ParseFrom(packet.BodyData); }
            catch { req = new SceneCastSkillCostMpCsReq(); }

            SceneCastSkillCostMpScRsp rsp = new SceneCastSkillCostMpScRsp
            {
                CastEntityId = req.CastEntityId,
            };
            
            // thought this would work :<
            // SceneCastSkillMpUpdateScNotify notify = new SceneCastSkillMpUpdateScNotify
            // {
            //     Mp = 5,
            //     CastEntityId = req.CastEntityId,
            // };

            await session.Send(CmdSceneType.CmdGetSceneMapInfoScRsp, rsp);
            // await session.Send(CmdSceneType.CmdSceneCastSkillMpUpdateScNotify, notify);
        }
    }
}
