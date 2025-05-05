using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Cmd;
using Serilog;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public partial class ClientConnection
    {
        private void HandlePacket(Packet packet)
        {
            Task.Run(async () =>
            {
                Log.Information("Received Cmd: {CmdId}", packet.CommandId);
                try
                {
                    switch (packet.CommandId)
                    {
                        // =============== AVATAR =============== 
                        case (ushort)CmdAvatarType.CmdGetAvatarDataCsReq:
                            await AvatarHandler.HandleGetAvatarData(SendPacket, packet);
                            break;

                        // =============== ITEM =============== 
                        case (ushort)CmdItemType.CmdGetBagCsReq:
                            await ItemHandler.HandleGetBag(SendPacket, packet);
                            break;

                        // =============== LINEUP =============== 
                        case (ushort)CmdLineupType.CmdGetAllLineupDataCsReq:
                            await LineupHandler.HandleGetAllLineupData(SendPacket, packet);
                            break;

                        case (ushort)CmdLineupType.CmdGetCurLineupDataCsReq:
                            await LineupHandler.HandleGetCurLineupData(SendPacket, packet);
                            break;

                        // =============== MISSION =============== 
                        case (ushort)CmdMissionType.CmdGetMissionStatusCsReq:
                            await MissionHandler.HandleGetMissionStatus(SendPacket, packet);
                            break;

                        // =============== PLAYER =============== 
                        case (ushort)CmdPlayerType.CmdGetBasicInfoCsReq:
                            await PlayerHandler.HandleGetBasicInfo(SendPacket, packet);
                            break;

                        case (ushort)CmdPlayerType.CmdGetMultiPathAvatarInfoCsReq:
                            await PlayerHandler.HandleGetMultiPathAvatarInfo(SendPacket, packet);
                            break;

                        case (ushort)CmdPlayerType.CmdPlayerGetTokenCsReq:
                            await PlayerHandler.HandlePlayerGetToken(SendPacket, packet);
                            break;

                        case (ushort)CmdPlayerType.CmdPlayerHeartBeatCsReq:
                            await PlayerHandler.HandlePlayerHeartBeat(SendPacket, packet);
                            break;

                        case (ushort)CmdPlayerType.CmdPlayerLoginFinishCsReq:
                            await PlayerHandler.HandlePlayerLoginFinish(SendPacket, packet);
                            break;

                        case (ushort)CmdPlayerType.CmdPlayerLoginCsReq:
                            await PlayerHandler.HandlePlayerLogin(SendPacket, packet);
                            break;

                        // =============== SCENE =============== 
                        case (ushort)CmdSceneType.CmdGetCurSceneInfoCsReq:
                            await SceneHandler.HandleGetCurSceneInfo(SendPacket, packet);
                            break;
                        case (ushort)CmdSceneType.CmdGetSceneMapInfoCsReq:
                            await SceneHandler.HandleGetSceneMapInfo(SendPacket, packet);
                            break;
                        case (ushort)CmdSceneType.CmdSceneEntityMoveCsReq:
                            await SceneHandler.HandleSceneEntityMove(SendPacket, packet);
                            break;

                        // =============== DEFAULT =============== 
                        default:
                            Log.Warning("Unhandled Cmd: {CmdId}", packet.CommandId);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("{Exception}", ex);
                }
            });
        }
    }
}
