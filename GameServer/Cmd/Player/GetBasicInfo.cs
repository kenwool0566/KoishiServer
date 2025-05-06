using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public partial class PlayerHandler
    {
        public static async Task CmdGetBasicInfoCsReq(Session session, Packet packet)
        {
            Gender mcGender;

            if (session.Persistent!.Trailblazer.Gender == "Girl") mcGender = Gender.Woman;
            else mcGender = Gender.Man;

            GetBasicInfoScRsp rsp = new GetBasicInfoScRsp
            {
                IsGenderSet = true,
                Gender = (uint)mcGender,
            };

            await session.Send(CmdPlayerType.CmdGetBasicInfoScRsp, rsp);
        }
    }
}
