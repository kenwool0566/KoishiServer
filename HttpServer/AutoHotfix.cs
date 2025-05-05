using Google.Protobuf;
using KoishiServer.Common.Config;
using KoishiServer.Common.Resource.Proto;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace KoishiServer.HttpServer
{
    // Credits: RobinSR's autohotfix for implementation reference and the proxy host
    public partial class DispatchHandler
    {
        private const string ProxyHost = "proxy1.neonteam.dev";
        private const string CNPRODHost = "prod-gf-cn-dp01.bhsr.com";
        private const string CNBETAHost = "beta-release01-cn.bhsr.com";
        private const string OSPRODHost = "prod-official-asia-dp01.starrails.com";
        private const string OSBETAHost = "beta-release01-asia.starrails.com";

        private static string SelectHost(string version)
        {
            if (version.StartsWith("CNP")) return CNPRODHost;
            else if (version.StartsWith("CNB")) return CNBETAHost;
            else if (version.StartsWith("OSP")) return OSPRODHost;
            else if (version.StartsWith("OSB")) return OSBETAHost;
            else return string.Empty;
        }

        private static string GetOfficialGatewayUri(string version, string dispatchSeed)
        {
            string host = SelectHost(version);
            if (string.IsNullOrEmpty(host)) return string.Empty;
            return $"https://{ProxyHost}/{host}/query_gateway?version={version}&dispatch_seed={dispatchSeed}&language_type=1&platform_type=2&channel_id=1&sub_channel_id=1&is_need_url=1&account_type=1";
        }

        public static async Task<(bool, HotfixData)> GetHotfix(HttpClient client, string version, string dispatchSeed)
        {
            Log.Information("[AutoHotfix] Starting AutoHotfix.");
            string gatewayUri = GetOfficialGatewayUri(version, dispatchSeed);
            // lmao
            bool doNotSave = true;

            if (gatewayUri == "")
            {
                Log.Error("[AutoHotfix] gatewayUri is empty. version-dispatchSeed={0}-{1}. Fallbacking.", version, dispatchSeed);
                return (doNotSave, new HotfixData());
            }

            Log.Information("[AutoHotfix] Gateway Uri: {Uri}", gatewayUri);

            try
            {
                HttpResponseMessage response = await client.GetAsync(gatewayUri);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    byte[] b64Decoded = Convert.FromBase64String(responseBody);
                    GateServer rsp = GateServer.Parser.ParseFrom(b64Decoded);

                    if (string.IsNullOrWhiteSpace(rsp.AssetBundleUrl) && string.IsNullOrWhiteSpace(rsp.ExResourceUrl))
                    {
                        throw new Exception("AssetBundleUrl and ExResourceUrl is empty.");
                    }

                    HotfixData newHotfix = new HotfixData
                    {
                        LuaUrl = rsp.LuaUrl,
                        AssetBundleUrl = rsp.AssetBundleUrl,
                        ExResourceUrl = rsp.ExResourceUrl,
                        IFixUrl = rsp.IfixUrl,
                        IFixVersion = rsp.IfixVersion,
                        MdkResVersion = rsp.MdkResVersion,
                    };
                    
                    Log.Information("[AutoHotfix] AutoHotfix finished successfully.");
                    return (!doNotSave, newHotfix);
                }
                else
                {
                    throw new Exception($"{gatewayUri} returned {response.StatusCode}");
                }
            }

            catch (Exception ex)
            {
                Log.Error("[AutoHotfix] AutoHotfix failed: {Exception}", ex);
                Log.Warning("[AutoHotfix] AutoHotfix caught an exception. Fallbacking.");
                return (doNotSave, new HotfixData());
            }
        }
    }
}
