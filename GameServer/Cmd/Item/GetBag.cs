using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class ItemHandler
    {
        public static async Task CmdGetBagCsReq(Session session, Packet packet)
        {
            await session.Send(CmdItemType.CmdGetBagScRsp, new GetBagScRsp());
        }
    }
}
