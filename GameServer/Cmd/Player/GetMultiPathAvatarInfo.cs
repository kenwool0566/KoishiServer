using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        private static readonly Dictionary<string, MultiPathAvatarType> MarchPathMap = new()
        {
            ["Preservation"] = MultiPathAvatarType.Mar7ThKnightType,
            ["TheHunt"] = MultiPathAvatarType.Mar7ThRogueType
        };

        private static readonly Dictionary<(string gender, string path), MultiPathAvatarType> McPathMap = new()
        {
            [("Boy", "Destruction")] = MultiPathAvatarType.BoyWarriorType,
            [("Boy", "Preservation")] = MultiPathAvatarType.BoyKnightType,
            [("Boy", "Harmony")] = MultiPathAvatarType.BoyShamanType,
            [("Boy", "Remembrance")] = MultiPathAvatarType.BoyMemoryType,

            [("Girl", "Destruction")] = MultiPathAvatarType.GirlWarriorType,
            [("Girl", "Preservation")] = MultiPathAvatarType.GirlKnightType,
            [("Girl", "Harmony")] = MultiPathAvatarType.GirlShamanType,
            [("Girl", "Remembrance")] = MultiPathAvatarType.GirlMemoryType,
        };

        public static async Task CmdGetMultiPathAvatarInfoCsReq(Session session, Packet packet)
        {
            string marchPath = session.Persistent!.MarchPath;
            string mcGender = session.Persistent!.Trailblazer.Gender;
            string mcPath = session.Persistent!.Trailblazer.Path;

            MultiPathAvatarType marchEnum = MarchPathMap.GetValueOrDefault(marchPath, MultiPathAvatarType.Mar7ThKnightType);
            MultiPathAvatarType mcEnum = McPathMap.GetValueOrDefault((mcGender, mcPath), MultiPathAvatarType.BoyWarriorType);

            GetMultiPathAvatarInfoScRsp rsp = new GetMultiPathAvatarInfoScRsp();
            rsp.CurAvatarPath.Add(1001, marchEnum);
            rsp.CurAvatarPath.Add(8001, mcEnum);

            await session.Send(CmdPlayerType.CmdGetMultiPathAvatarInfoScRsp, rsp);
        }
    }
}
