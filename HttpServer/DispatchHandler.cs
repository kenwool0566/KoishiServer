using Google.Protobuf;
using KoishiServer.Common.Config;
using KoishiServer.Common.Resource.Proto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KoishiServer.HttpServer
{
    public class DispatchHandler
    {
        public static void MapDispatchRoutes(WebApplication app)
        {
            app.Map("/query_dispatch", HandleQueryDispatch);
            app.Map("/query_gateway", HandleQueryGateway);
        }

        private static async Task HandleQueryDispatch(HttpContext context)
        {
            ServerConfig serverConfig = context.RequestServices.GetRequiredService<ServerConfig>();

            RegionInfo regionInfo = new RegionInfo
            {
                Name = "KoishiServer",
                Title = "KoishiServer",
                DispatchUrl = serverConfig.DispatchUrl,
                EnvType = "2",
                DisplayName = "KoishiServer",
                Msg = "OK"
            };

            Dispatch dispatch = new Dispatch
            {
                RegionList = { regionInfo },
                Msg = "OK",
                TopSeverRegionName = "KoishiServer"
            };

            byte[] packedData = dispatch.ToByteArray();
            string b64Encoded = Convert.ToBase64String(packedData);
            await context.Response.WriteAsync(b64Encoded);
        }

        private static async Task HandleQueryGateway(HttpContext context)
        {
            String version = context.Request.Query["version"].ToString();
            ServerConfig serverConfig = context.RequestServices.GetRequiredService<ServerConfig>();
            HotfixConfig hotfixConfig = context.RequestServices.GetRequiredService<HotfixConfig>();

            hotfixConfig.HotfixData.TryGetValue(version, out HotfixData? hotfix);
            hotfix ??= hotfixConfig.HotfixData.GetValueOrDefault("*", new HotfixData());

            GateServer gateServer = new GateServer
            {
                UseTcp = serverConfig.UseTcp,
                Ip = serverConfig.Host,
                Port = serverConfig.GameServerPort,
                
                LuaUrl = hotfix.LuaUrl,
                AssetBundleUrl = hotfix.AssetBundleUrl,
                ExResourceUrl = hotfix.ExResourceUrl,
                IfixUrl = hotfix.IFixUrl,
                IfixVersion = hotfix.IFixVersion,
                MdkResVersion = hotfix.MdkResVersion,

                EnableDesignDataVersionUpdate = true,
                EnableVersionUpdate = true,
                EnableUploadBattleLog = true,
                NetworkDiagnostic = true,
                CloseRedeemCode = true,
                EnableAndroidMiddlePackage = true,
                EnableWatermark = true,
                EventTrackingOpen = true,
                EnableSaveReplayFile = true
            };

            byte[] packedData = gateServer.ToByteArray();
            string b64Encoded = Convert.ToBase64String(packedData);
            await context.Response.WriteAsync(b64Encoded);
        }
    }
}
