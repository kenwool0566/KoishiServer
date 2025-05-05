using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

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
            return ConfigLoader.FromFile<HotfixConfig>(HotfixConfigFilePath);
        }

        public static async Task InsertHotfixDataAsync(string version, HotfixData newData)
        {
            try
            {
                string json = await File.ReadAllTextAsync(HotfixConfigFilePath);
                HotfixConfig? result = JsonConvert.DeserializeObject<HotfixConfig>(json);

                if (result == null) throw new InvalidDataException($"Failed to deserialize {HotfixConfigFilePath}.");

                result.HotfixData[version] = newData;

                string updatedJson = JsonConvert.SerializeObject(result, Formatting.Indented);
                await File.WriteAllTextAsync(HotfixConfigFilePath, updatedJson);
            }

            catch (Exception ex)
            {
                Log.Error("Failed inserting HotfixData to {File}: {Exception}", HotfixConfigFilePath, ex);
            }
        }
    }
}
