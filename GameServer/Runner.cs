using KoishiServer.Common.Config;
using KoishiServer.GameServer.Network;
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
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            string socketAddr = $"{serverConfig.Host}:{port}";

            Log.Information("GameServer is listening on {Addr}.", socketAddr);
            TcpServer server = new TcpServer();
            server.Start(endpoint);
        }
    }
}
