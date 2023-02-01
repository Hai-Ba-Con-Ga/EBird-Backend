using EBird.Application.Interfaces;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using MailKit;

namespace EBird.Api.Configurations
{
    public static class ConfigureApplicationService
    {
        public static void AddAppServices (this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddScoped<IBirdTypeService, BirdTypeService>();
            services.AddScoped<IBirdService, BirdService>();
            services.AddSingleton<ICloudStorage, GoogleCloudStorageService>();
            services.AddScoped<IFileServices, FileServices>();

        }
    }
}
