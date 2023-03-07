using EBird.Application.AppConfig;

namespace EBird.Api.Configurations;
public static class ConfigureSetting
{

    public static IServiceCollection AddSettingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleSetting>(configuration.GetSection(GoogleSetting.GoogleSettingString));
        services.Configure<MailSetting>(configuration.GetSection(MailSetting.MailSettingString));
        services.Configure<AppSetting>(configuration.GetSection(AppSetting.AppSettingString));
        services.Configure<VnpayConfig>(configuration.GetSection(VnpayConfig.VnpayConfigString));

        return services;
    }
}