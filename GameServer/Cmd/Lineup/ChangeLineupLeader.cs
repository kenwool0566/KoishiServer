using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class LineupHandler
    {
        public static async Task CmdChangeLineupLeaderCsReq(Session session, Packet packet)
        {
            ChangeLineupLeaderCsReq req;
            try { req = ChangeLineupLeaderCsReq.Parser.ParseFrom(packet.BodyData); }
            catch { req = new ChangeLineupLeaderCsReq(); }

            ChangeLineupLeaderScRsp rsp = new ChangeLineupLeaderScRsp
            {
                Slot = req.Slot,
            };

            session.Persistent!.SetLineupLeader((byte)req.Slot);
            await session.Send(CmdLineupType.CmdChangeLineupLeaderScRsp, rsp);
        }
    }
}
