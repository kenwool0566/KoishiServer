using Google.Protobuf;
using KoishiServer.Common.Config;
using KoishiServer.Common.Resource.Proto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KoishiServer.HttpServer
{
    public partial class DispatchHandler
    {
        public static void MapDispatchRoutes(WebApplication app)
        {
            app.MapGet("/query_dispatch", HandleQueryDispatch);
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
            if (context.Request.Method == HttpMethods.Head)
            {
                await context.Response.CompleteAsync();
                return;
            }

            else if (context.Request.Method != HttpMethods.Get)
            {
                context.Response.StatusCode = 405;
                await context.Response.CompleteAsync();
                return;
            }

            string version = context.Request.Query["version"].ToString();
            ServerConfig serverConfig = context.RequestServices.GetRequiredService<ServerConfig>();
            HotfixConfig hotfixConfig = context.RequestServices.GetRequiredService<HotfixConfig>();

            hotfixConfig.HotfixData.TryGetValue(version, out HotfixData? hotfixData);

            if (hotfixData != null)
            {
                string encodedGateServer = CreateEncodedGateServer(serverConfig, hotfixData);
                await context.Response.WriteAsync(encodedGateServer);
            }

            else if (hotfixData == null && serverConfig.EnableAutoHotfix == false)
            {
                hotfixData = hotfixConfig.HotfixData.GetValueOrDefault("*", new HotfixData());
                string encodedGateServer = CreateEncodedGateServer(serverConfig, hotfixData);
                await context.Response.WriteAsync(encodedGateServer);
            }

            else
            {
                IHttpClientFactory httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                HttpClient client = httpClientFactory.CreateClient();

                string dispatchSeed = context.Request.Query["dispatch_seed"].ToString();
                (bool doNotSave, hotfixData) = await GetHotfix(client, version, dispatchSeed);

                string encodedGateServer = CreateEncodedGateServer(serverConfig, hotfixData);
                await context.Response.WriteAsync(encodedGateServer);

                if (doNotSave == false)
                {
                    // this isn't a good idea but it's a singleplayer server so whatever.
                    hotfixConfig.HotfixData[version] = hotfixData;
                    await HotfixConfigLoader.InsertHotfixDataAsync(version, hotfixData);
                }
            }
        }

        private static string CreateEncodedGateServer(ServerConfig serverConfig, HotfixData hotfix)
        {
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
            return Convert.ToBase64String(packedData);
        }
    }
}
