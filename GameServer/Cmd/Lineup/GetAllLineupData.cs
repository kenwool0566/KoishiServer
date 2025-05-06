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
            List<LineupAvatar> lineupAvatars = session.Persistent!.Lineup
                .Where(kvp => kvp.Value != null)
                .Select(kvp => CreateLineupAvatar(kvp.Key, kvp.Value!.Id))
                .ToList();

            GetAllLineupDataScRsp rsp = new GetAllLineupDataScRsp
            {
                LineupList = { CreateLineupInfo(lineupAvatars) },
            };

            await session.Send(CmdLineupType.CmdGetAllLineupDataScRsp, rsp);
        }

        private static LineupInfo CreateLineupInfo(List<LineupAvatar> lineupAvatars)
        {
            return new LineupInfo
            {
                ExtraLineupType = ExtraLineupType.LineupNone,
                Name = "KoishiTeam",
                Mp = 5,
                MaxMp = 5,
                AvatarList = { lineupAvatars },
            };
        }

        private static LineupAvatar CreateLineupAvatar(uint slot, uint avatarId)
        {
            return new LineupAvatar
            {
                Id = avatarId,
                Hp = 10000,
                Satiety = 100,
                AvatarType = AvatarType.AvatarFormalType,
                SpBar = new SpBarInfo
                {
                    CurSp = 10000,
                    MaxSp = 10000,
                },
                Slot = slot,
            };
        }
    }
}
