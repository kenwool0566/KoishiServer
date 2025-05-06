using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class AvatarHandler
    {
        private static readonly uint[] UnlockedAvatars = new uint[]
        {
            1002, 1003, 1004, 1005, 1006, 1008, 1009, 1013, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 1108,
            1109, 1110, 1111, 1112, 1201, 1202, 1203, 1204, 1205, 1206, 1207, 1208, 1209, 1210, 1211, 1212,
            1213, 1214, 1215, 1217, 1301, 1302, 1303, 1304, 1305, 1306, 1307, 1308, 1309, 1312, 1315, 1310,
            1314, 1218, 1221, 1220, 1222, 1223, 1317, 1313, 1225, 1402, 1401, 1404, 1403, 1405, 1407, 1406,
            1409,

            1001, 1221,
        };

        public static async Task CmdGetAvatarDataCsReq(Session session, Packet packet)
        {
            List<uint> tbIds = session.Persistent!.GetTrailblazerIds();

            List<Avatar> tbAvatarList = tbIds
                .Select(id => CreateMaxAvatar(id))
                .ToList();

            List<Avatar> avatarList = UnlockedAvatars
                .Select(id => CreateMaxAvatar(id))
                .ToList();
            
            tbAvatarList.AddRange(avatarList);

            GetAvatarDataScRsp rsp = new GetAvatarDataScRsp {
                AvatarList = { tbAvatarList },
                IsGetAll = true,
            };

            await session.Send(CmdAvatarType.CmdGetAvatarDataScRsp, rsp);
        }

        private static Avatar CreateMaxAvatar(uint avatarId)
        {
            List<AvatarSkillTree> avatarSkillTrees = Enumerable.Range(1, 4)
                .Select(n => new AvatarSkillTree
                {
                    PointId = avatarId * 1000 + (uint)n,
                    Level = 1
                })
                .ToList();

            return new Avatar
            {
                Rank = 6,
                Promotion = 6,
                Level = 80,
                BaseAvatarId = avatarId,
                FirstMetTimeStamp = 1712924677,
                SkilltreeList = { avatarSkillTrees },
            };
        }
    }
}
