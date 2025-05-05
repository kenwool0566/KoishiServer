using KoishiServer.Common.Resource.Proto;
using KoishiServer.GameServer.Cmd;
using Serilog;
using System;
using System.Threading.Tasks;

namespace KoishiServer.GameServer.Network
{
    public static class Handler
    {
        private static readonly Dictionary<ushort, Func<Session, Packet, Task>> _handlers = new();

        public static void RegisterAll()
        {
            Log.Information("Registering GameServer handlers.");

            /** 
                CmdAvatarType
            **/
            Register(CmdAvatarType.CmdGetAvatarDataCsReq, AvatarHandler.CmdGetAvatarDataCsReq);

            /** 
                CmdLineupType
            **/
            Register(CmdItemType.CmdGetBagCsReq, ItemHandler.CmdGetBagCsReq);

            /** 
                CmdLineupType
            **/
            Register(CmdLineupType.CmdGetAllLineupDataCsReq, LineupHandler.CmdGetAllLineupDataCsReq);
            Register(CmdLineupType.CmdGetCurLineupDataCsReq, LineupHandler.CmdGetCurLineupDataCsReq);

            /** 
                CmdMissionType
            **/
            Register(CmdMissionType.CmdGetMissionStatusCsReq, MissionHandler.CmdGetMissionStatusCsReq);

            /** 
                CmdPlayerType
            **/
            Register(CmdPlayerType.CmdGetBasicInfoCsReq, PlayerHandler.CmdGetBasicInfoCsReq);
            Register(CmdPlayerType.CmdGetMultiPathAvatarInfoCsReq, PlayerHandler.CmdGetMultiPathAvatarInfoCsReq);
            Register(CmdPlayerType.CmdPlayerGetTokenCsReq, PlayerHandler.CmdPlayerGetTokenCsReq);
            Register(CmdPlayerType.CmdPlayerHeartBeatCsReq, PlayerHandler.CmdPlayerHeartBeatCsReq);
            Register(CmdPlayerType.CmdPlayerLoginCsReq, PlayerHandler.CmdPlayerLoginCsReq);
            Register(CmdPlayerType.CmdPlayerLoginFinishCsReq, PlayerHandler.CmdPlayerLoginFinishCsReq);

            /** 
                CmdSceneType
            **/
            Register(CmdSceneType.CmdGetCurSceneInfoCsReq, SceneHandler.CmdGetCurSceneInfoCsReq);
            Register(CmdSceneType.CmdGetSceneMapInfoCsReq, SceneHandler.CmdGetSceneMapInfoCsReq);
            Register(CmdSceneType.CmdSceneCastSkillCostMpCsReq, SceneHandler.CmdSceneCastSkillCostMpCsReq);
            Register(CmdSceneType.CmdSceneEntityMoveCsReq, SceneHandler.CmdSceneEntityMoveCsReq);

            Log.Information("Registered {Count} GameServer handlers.", _handlers.Count);
        }

        private static void Register<T>(T commandType, Func<Session, Packet, Task> HandleFn) 
            where T : Enum
        {
            _handlers[Convert.ToUInt16(commandType)] = HandleFn;
        }

        public static async Task HandlePacket(Session session, Packet packet)
        {
            Log.Information("Got Cmd: {CmdId}", packet.CommandId);
            if (_handlers.TryGetValue(packet.CommandId, out Func<Session, Packet, Task>? HandleFn))
            {
                try
                {
                    await HandleFn(session, packet);
                }
                catch (Exception ex)
                {
                    Log.Error("{Exception}", ex);
                }
            }
            else
            {
                Log.Warning("Unhandled Cmd: {CmdId}", packet.CommandId);
            }
        }
    }
}
