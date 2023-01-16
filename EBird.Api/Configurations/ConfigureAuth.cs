using EBird.Application.AppConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EBird.Api.Configurations
{
    public static class ConfigureAuth
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSetting jwt = configuration.GetSection(JwtSetting.JwtSettingString).Get<JwtSetting>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>

            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = jwt.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwt.IssuerSigningKey)),
                    ValidateIssuer = jwt.ValidateIssuer,
                    ValidateAudience = jwt.ValidateAudience,
                };
            }
            );
            return services;
        }
    }
}
