using System.Reflection;
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
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddSingleton<ICloudStorage, GoogleCloudStorageService>();
            services.AddScoped<IFileServices, FileServices>();         
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
