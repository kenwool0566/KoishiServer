using KoishiServer.Common.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace KoishiServer.HttpServer
{
    public class Runner
    {
        public static void Start(ServerConfig serverConfig, HotfixConfig hotfixConfig)
        {
            try
            {
                Log.Information("Starting HttpServer.");

                WebApplicationBuilder builder = WebApplication.CreateBuilder();
                builder.Logging.ClearProviders();
                builder.Services.AddHttpClient();
                builder.Services.AddSingleton(serverConfig);
                builder.Services.AddSingleton(hotfixConfig);
                builder.Services.AddRouting();

                WebApplication app = builder.Build();
                app.Use(async (context, next) =>
                {
                    await next.Invoke();
                    int statusCode = context.Response.StatusCode;
                    string method = context.Request.Method;
                    string uri = context.Request.Path + context.Request.QueryString;
                    Log.Information("{Status} - {Method} {Uri}", statusCode, method, uri);
                });

                AuthHandler.MapAuthRoutes(app);
                DispatchHandler.MapDispatchRoutes(app);

                string socketAddr = $"{serverConfig.Host}:{serverConfig.HttpServerPort}";
                string url = $"http://{socketAddr}";
                Log.Information("HttpServer is listening on {Addr}.", socketAddr);
                app.Run(url);
            }

            catch (Exception ex)
            {
                Log.Fatal("HttpServer terminated unexpectedly: {Exception}", ex);
            }
        }
    }
}
