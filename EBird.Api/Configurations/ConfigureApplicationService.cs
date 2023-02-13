using System.Reflection;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
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
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IRequestService, RequestService>();
            //register Validation services
            services.AddScoped<IUnitOfValidation, UnitOfValidation>();
            //register AutoMapper services
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
