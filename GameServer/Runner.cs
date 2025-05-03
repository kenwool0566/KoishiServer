using KoishiServer.Common.Config;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace KoishiServer.GameServer 
{
    public static class Runner
    {
        public static void Start(ServerConfig serverConfig)
        {
            Log.Information("Starting GameServer.");

            IPAddress ip = IPAddress.Parse(serverConfig.Host);
            ushort port = serverConfig.GameServerPort;

            string socketAddr = $"{serverConfig.Host}:{port}";
            Log.Information("GameServer is listening on {Addr}.", socketAddr);
            Task.Run(() => Log.Warning("GameServer is unimplemented."));
        }
    }
}
