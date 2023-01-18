using EBird.Application.Services;
using EBird.Application.Services.IServices;

namespace EBird.Api.Configurations
{
    public static class ConfigureApplicationService
    {
        public static void AddAppServices (this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
        }
    }
}
