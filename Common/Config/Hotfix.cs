using Newtonsoft.Json;

namespace KoishiServer.Common.Config
{
    public class HotfixConfig
    {
        [JsonProperty("HotfixData")]
        public Dictionary<string, HotfixData> HotfixData { get; set; } = new()
        {
            ["*"] = new HotfixData()
        };
    }

    public class HotfixData
    {
        [JsonProperty("LuaUrl")]
        public string LuaUrl { get; set; } = "";

        [JsonProperty("AssetBundleUrl")]
        public string AssetBundleUrl { get; set; } = "";

        [JsonProperty("ExResourceUrl")]
        public string ExResourceUrl { get; set; } = "";

        [JsonProperty("IFixUrl")]
        public string IFixUrl { get; set; } = "";

        [JsonProperty("IFixVersion")]
        public string IFixVersion { get; set; } = "0";

        [JsonProperty("MdkResVersion")]
        public string MdkResVersion { get; set; } = "";
    }

    public static class HotfixConfigLoader
    {
        private static readonly string HotfixConfigFilePath = "HotfixConfig.json";

        public static HotfixConfig LoadConfig()
        {
            return ConfigLoader.LoadConfig<HotfixConfig>(HotfixConfigFilePath);
        }
    }
}
