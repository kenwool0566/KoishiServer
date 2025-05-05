using KoishiServer.Common.Resource.Proto;
using KoishiServer.Common.Util;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdPlayerHeartBeatCsReq(Session session, Packet packet)
        {
            PlayerHeartBeatCsReq req;
            try { req = PlayerHeartBeatCsReq.Parser.ParseFrom(packet.BodyData); }
            catch { req = new PlayerHeartBeatCsReq(); }

            PlayerHeartBeatScRsp rsp = new PlayerHeartBeatScRsp
            {
                ClientTimeMs = req.ClientTimeMs,
                ServerTimeMs = (ulong)TimeUtils.GetTimestampMs(),
            };

            await session.Send(CmdPlayerType.CmdPlayerHeartBeatScRsp, rsp);
        }
    }
}
