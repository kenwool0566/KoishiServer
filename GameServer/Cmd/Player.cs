using Google.Protobuf;
using KoishiServer.Common.Resource.Proto;
using KoishiServer.Common.Util;
using KoishiServer.GameServer.Network;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Cmd
{
    public class PlayerHandler
    {
        public static async Task HandleGetBasicInfo(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdPlayerType.CmdGetBasicInfoScRsp;
            GetBasicInfoScRsp rsp = new GetBasicInfoScRsp();
            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandleGetMultiPathAvatarInfo(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdPlayerType.CmdGetMultiPathAvatarInfoScRsp;
            GetMultiPathAvatarInfoScRsp rsp = new GetMultiPathAvatarInfoScRsp();
            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandlePlayerGetToken(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdPlayerType.CmdPlayerGetTokenScRsp;
            PlayerGetTokenScRsp rsp = new PlayerGetTokenScRsp
            {
                Uid = 1,
                Msg = "OK",
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandlePlayerHeartBeat(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdPlayerType.CmdPlayerHeartBeatScRsp;

            PlayerHeartBeatCsReq req;
            try { req = PlayerHeartBeatCsReq.Parser.ParseFrom(inPacket.BodyData); }
            catch { req = new PlayerHeartBeatCsReq(); }

            PlayerHeartBeatScRsp rsp = new PlayerHeartBeatScRsp
            {
                ClientTimeMs = req.ClientTimeMs,
                ServerTimeMs = (ulong)TimeUtils.GetTimestampMs(),
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandlePlayerLogin(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdPlayerType.CmdPlayerLoginScRsp;

            PlayerBasicInfo basicInfo = new PlayerBasicInfo
            {
                Nickname = "Koishi",
                Level = 70,
                Mcoin = 1,
                Hcoin = 2,
                Scoin = 3,
                WorldLevel = 6,
                Stamina = 300,
            };

            PlayerLoginScRsp rsp = new PlayerLoginScRsp
            {
                BasicInfo = basicInfo,
                ServerTimestampMs = (ulong)TimeUtils.GetTimestampMs(),
                Stamina = 300,
            };

            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }

        public static async Task HandlePlayerLoginFinish(Func<Packet, Task> SendPacket, Packet inPacket)
        {
            ushort rspCmd = (ushort)CmdPlayerType.CmdPlayerLoginFinishScRsp;
            PlayerLoginFinishScRsp rsp = new PlayerLoginFinishScRsp();
            Packet outPacket = Packet.Headless(rspCmd, rsp.ToByteArray());
            await SendPacket(outPacket);
        }
    }
}
