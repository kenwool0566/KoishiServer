using KoishiServer.Common.Config;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KoishiServer.HttpServer
{
    public class SRToolsHandler
    {
        public static void MapSRToolsRoutes(WebApplication app)
        {
            app.Map("/srtools", HandleSRTools);
        }

        private static async Task HandleSRTools(HttpContext context)
        {
            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            context.Response.Headers["Access-Control-Allow-Methods"] = "POST, OPTIONS";
            context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type";
            
            if (context.Request.Method == HttpMethods.Options)
            {
                await context.Response.WriteAsJsonAsync(CreateRsp(200, "OK"));
                return;
            }

            if (context.Request.Method != HttpMethods.Post)
            {
                context.Response.StatusCode = 405;
                await context.Response.CompleteAsync();
                return;
            }

            try
            {
                string body = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();
                
                if (string.IsNullOrWhiteSpace(body))
                {
                    await context.Response.WriteAsJsonAsync(CreateRsp(400, "Request body is empty."));
                    return;
                }

                SRToolDataReq req = ConfigLoader.FromString<SRToolDataReq>(body);

                if (req.Data == null)
                {
                    await context.Response.WriteAsJsonAsync(CreateRsp(200, "OK"));
                    return;
                }

                await SRToolsConfigLoader.SaveToFileAsync(req.Data);
                await context.Response.WriteAsJsonAsync(CreateRsp(200, "OK"));
            }
            
            catch (Exception ex)
            {
                Log.Error("Error handling SRTools request: {Exception}", ex);
                await context.Response.WriteAsJsonAsync(CreateRsp(500, "Internal Server Error"));
            }
        }

        private static SRToolDataRsp CreateRsp(int statusCode, string msg)
        {
            return new SRToolDataRsp
            {
                Status = statusCode,
                Message = msg,
            };
        }
    }
}
