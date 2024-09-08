namespace BancolombiaStarter.Backend.Api.Extension
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using BancolombiaStarter.Backend.Infrastructure.Authorization;

    public static class HttpContextExtensions
    {
        public static string GetUserIdFromToken(this HttpContext httpContext, JwtService jwtService)
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return jwtService.GetUserIdFromToken(token);
        }
    }
}
