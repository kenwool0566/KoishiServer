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
            GetCurLineupDataScRsp rsp = new GetCurLineupDataScRsp
            {
                Lineup = YunliOnly(),
            };

            await session.Send(CmdLineupType.CmdGetCurLineupDataScRsp, rsp);
        }
    }
}
