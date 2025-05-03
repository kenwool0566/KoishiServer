using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KoishiServer.HttpServer
{
    public class AuthHandler
    {
        public static void MapAuthRoutes(WebApplication app)
        {
            app.Map("/{game_biz}/mdk/shield/api/login", HandleMdkShieldApi);
            app.Map("/{game_biz}/mdk/shield/api/verify", HandleMdkShieldApi);
            app.Map("/{game_biz}/combo/granter/login/v2/login", HandleComboGranterLogin);
            app.Map("/account/risky/api/check", HandleRiskyCheck);
        }

        private static async Task HandleRiskyCheck(HttpContext context)
        {
            const string response = "{\"data\":{},\"message\":\"OK\",\"retcode\":0}";
            await context.Response.WriteAsync(response);
        }

        private static async Task HandleMdkShieldApi(HttpContext context)
        {
            const string response = "{\"data\":{\"account\":{\"area_code\":\"**\",\"country\":\"RU\",\"email\":\"koishi@gensok.yo\",\"is_email_verify\":\"1\",\"token\":\"C\",\"uid\":\"1\"},\"device_grant_required\":false,\"reactivate_required\":false,\"realperson_required\":false,\"safe_mobile_required\":false},\"message\":\"OK\",\"retcode\":0}";
            await context.Response.WriteAsync(response);
        }

        private static async Task HandleComboGranterLogin(HttpContext context)
        {
            const string response = "{\"data\":{\"account_type\":1,\"combo_id\":\"1\",\"combo_token\":\"C\",\"data\":\"{\\\"guest\\\":false}\",\"heartbeat\":false,\"open_id\":\"1\"},\"message\":\"OK\",\"retcode\":0}";
            await context.Response.WriteAsync(response);
        }
    }
}
