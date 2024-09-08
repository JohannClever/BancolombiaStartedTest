using Microsoft.Extensions.DependencyInjection;
using BancolombiaStarter.Backend.Infrastructure.Authorization;

namespace BancolombiaStarter.Backend.Infrastructure.Extensions
{
    public static  class JwtExtension
    {
        public static IServiceCollection AddJwtServices(this IServiceCollection svc)
        {
            svc.AddScoped<JwtValidationService>();
            svc.AddScoped<JwtService>();
            return svc;
            
        }
    }
}
