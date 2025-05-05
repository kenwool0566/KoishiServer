using KoishiServer.Common.Config;
using KoishiServer.GameServer.Network;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace KoishiServer.GameServer
{
    public static class Runner
    {
        public static async Task Start(ServerConfig serverConfig)
        {
            try
            {
                Handler.RegisterAll();
                Log.Information("Starting GameServer.");
                
                IPAddress ip = IPAddress.Parse(serverConfig.Host);
                ushort port = serverConfig.GameServerPort;

                IPEndPoint endpoint = new IPEndPoint(ip, port);

                string socketAddr = $"{serverConfig.Host}:{port}";
                Log.Information("GameServer is listening on {Addr}.", socketAddr);

                TcpServer server = new TcpServer(endpoint);
                await server.StartAsync();
            }

            catch (Exception ex)
            {
                Log.Fatal("GameServer terminated unexpectedly: {Exception}", ex);
            }
        }
    }
}
