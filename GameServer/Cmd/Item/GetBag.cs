using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class ItemHandler
    {
        public static async Task CmdGetBagCsReq(Session session, Packet packet)
        {
            GetBagScRsp rsp = new GetBagScRsp
            {
                MaterialList = {
                    new Material { Tid = 101, Num = 9999 },
                    new Material { Tid = 102, Num = 9999 },
                    new Material { Tid = 400006, Num = 100 },
                },
            };

            await session.Send(CmdItemType.CmdGetBagScRsp, rsp);
        }
    }
}
