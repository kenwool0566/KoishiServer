using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class LineupHandler
    {
        public static async Task CmdGetAllLineupDataCsReq(Session session, Packet packet)
        {
            GetAllLineupDataScRsp rsp = new GetAllLineupDataScRsp
            {
                LineupList = { YunliOnly() },
            };

            await session.Send(CmdLineupType.CmdGetAllLineupDataScRsp, rsp);
        }

        private static LineupInfo YunliOnly()
        {
            LineupAvatar lineupAvatar = new LineupAvatar
            {
                Id = 1221,
                Hp = 10000,
                AvatarType = AvatarType.AvatarFormalType,
                SpBar = new SpBarInfo
                {
                    CurSp = 10000,
                    MaxSp = 10000,
                },
            };

            LineupInfo lineupInfo = new LineupInfo
            {
                Name = "KoishiTeam",
                AvatarList = { lineupAvatar },
                Mp = 5,
                MaxMp = 5,
                PlaneId = 20101,
            };

            return lineupInfo;
        }
    }
}
