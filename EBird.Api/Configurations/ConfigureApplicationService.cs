using System.Reflection;
using EBird.Api.RuleSettings;
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
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchDetailService, MatchDeatailService>();
            services.AddScoped<IGroupMemberService, GroupMemberService>();
            services.AddScoped<IMatchingService, MatchingService>();
            //register Validation services
            services.AddScoped<IUnitOfValidation, UnitOfValidation>();
            //register AutoMapper services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //config rule settings
            services.AddSingleton<IRuleSetting, RuleSetting>();
            services.AddScoped<IScoringService, ScoringService>();
            services.AddScoped<IReportServices, ReportServices>();

            services.AddScoped<IPostService, PostService>();

            services.AddScoped<IMapsServices, MapsService>();
            services.AddScoped<IPaymentService, VnPayService>();
            services.AddHttpClient<IMapsServices, MapsService>();
            services.AddScoped<IVipService, VipService>();
            
        }
    }
}
