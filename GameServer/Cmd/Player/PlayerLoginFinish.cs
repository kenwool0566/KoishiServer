using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdPlayerLoginFinishCsReq(Session session, Packet packet)
        {
            List<uint> contentIds = new List<uint>
            {
                200001, 200002, 200003, 200004, 150017, 150015, 150021, 150018, 130011, 130012, 130013,
            };

            List<ContentPackageInfo> contentPackageList = contentIds
                .Select(id => new ContentPackageInfo
                {
                    Status = ContentPackageStatus.Finished,
                    ContentId = id,
                })
                .ToList();

            ContentPackageSyncDataScNotify notify = new ContentPackageSyncDataScNotify
            {
                Data = new ContentPackageData
                {
                    ContentPackageList = { contentPackageList },
                },
            };

            await session.Send(CmdContentPackageType.CmdContentPackageSyncDataScNotify, notify);
            await session.Send(CmdPlayerType.CmdPlayerLoginFinishScRsp, new PlayerLoginFinishScRsp());
        }
    }
}

