using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class LineupHandler
    {
        public static async Task CmdGetCurLineupDataCsReq(Session session, Packet packet)
        {
            List<LineupAvatar> lineupAvatars = session.Persistent!.Lineup
                .Where(kvp => kvp.Value != null)
                .Select(kvp => CreateLineupAvatar(kvp.Key, kvp.Value!.Id))
                .ToList();

            GetCurLineupDataScRsp rsp = new GetCurLineupDataScRsp
            {
                Lineup = CreateLineupInfo(lineupAvatars),
            };

            await session.Send(CmdLineupType.CmdGetCurLineupDataScRsp, rsp);
        }
    }
}
