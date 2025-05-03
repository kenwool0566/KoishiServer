using KoishiServer.Common.Config;
using Serilog;
using Serilog.Events;
using System.Threading;

namespace KoishiServer.Program
{
    class EntryPoint
    {
        static int Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            
            string License = "ISC License";
            short Year = 2025;
            string Author = "kenwool0566";
            string Repository = "https://github.com/kenwool0566/KoishiServer";
            
            Log.Information("{0}{1}{2}", "+", new string('~', 50), "+");
            Log.Information("{0} {1,-48} {2}", "|", "Welcome to KoishiServer.", "|");
            Log.Information("{0} {1,-48} {2}", "|", $"Copyright {Year} {Author} - {License}", "|");
            Log.Information("{0} {1,-48} {2}", "|", $"{Repository}", "|");
            Log.Information("{0}{1}{2}", "+", new string('~', 50), "+");

            ServerConfig serverConfig = ServerConfigLoader.LoadConfig();
            HotfixConfig hotfixConfig = HotfixConfigLoader.LoadConfig();

            Thread httpThread = new Thread(() => KoishiServer.HttpServer.Runner.Start(serverConfig, hotfixConfig));
            Thread gameThread = new Thread(() => KoishiServer.GameServer.Runner.Start(serverConfig));

            httpThread.Start();
            gameThread.Start();

            return 0;
        }
    }
}
