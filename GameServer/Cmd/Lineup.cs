using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public class LineupHandler
    {
        public static async Task HandleGetAllLineupData(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdLineupType.CmdGetAllLineupDataScRsp;

            GetAllLineupDataScRsp rsp = new GetAllLineupDataScRsp
            {
                LineupList = { YunliOnly() },
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandleGetCurLineupData(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdLineupType.CmdGetCurLineupDataScRsp;

            GetCurLineupDataScRsp rsp = new GetCurLineupDataScRsp
            {
                Lineup = YunliOnly(),
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        private static LineupInfo YunliOnly()
        {
            SpBarInfo spBarInfo = new SpBarInfo
            {
                CurSp = 10000,
                MaxSp = 10000,
            };

            LineupAvatar lineupAvatar = new LineupAvatar
            {
                Id = 1221,
                Hp = 10000,
                AvatarType = AvatarType.AvatarFormalType,
                SpBar = spBarInfo
            };

            LineupInfo lineupInfo = new LineupInfo
            {
                Name = "KoishiTeam",
                AvatarList = { lineupAvatar },
            };

            return lineupInfo;
        }
    }
}
