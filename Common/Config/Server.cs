using Newtonsoft.Json;

namespace KoishiServer.Common.Config
{
    public class ServerConfig
    {
        [JsonProperty("Host")]
        public string Host { get; set; } = "127.0.0.1";

        [JsonProperty("HttpServerPort")]
        public ushort HttpServerPort { get; set; } = 21000;

        [JsonProperty("GameServerPort")]
        public ushort GameServerPort { get; set; } = 23301;

        [JsonProperty("DispatchUrl")]
        public string DispatchUrl { get; set; } = "http://127.0.0.1:21000/query_gateway";

        [JsonProperty("UseTcp")]
        public bool UseTcp { get; set; } = true;

        [JsonProperty("EnableAutoHotfix")]
        public bool EnableAutoHotfix { get; set; } = false;
    }

    public static class ServerConfigLoader
    {
        private const string ServerConfigFilePath = "ServerConfig.json";

        public static ServerConfig LoadConfig()
        {
            return ConfigLoader.FromFile<ServerConfig>(ServerConfigFilePath);
        }
    }
}
