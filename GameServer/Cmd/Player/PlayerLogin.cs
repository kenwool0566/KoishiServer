using KoishiServer.Common.Resource.Proto;
using KoishiServer.Common.Util;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdPlayerLoginCsReq(Session session, Packet packet)
        {
            PlayerLoginScRsp rsp = new PlayerLoginScRsp
            {
                BasicInfo = new PlayerBasicInfo
                {
                    Nickname = "Koishi",
                    Level = 70,
                    Mcoin = 1,
                    Hcoin = 2,
                    Scoin = 3,
                    WorldLevel = 6,
                    Stamina = 300,
                },
                ServerTimestampMs = (ulong)TimeUtils.GetTimestampMs(),
                Stamina = 300,
            };

            await session.Send(CmdPlayerType.CmdPlayerLoginScRsp, rsp);
        }
    }
}
