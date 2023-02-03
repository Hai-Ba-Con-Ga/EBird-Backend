using AutoWrapper;
using EBird.Api.Configurations;
using EBird.Application.AppConfig;
using EBird.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "BirdAllowSpecificOrigins",
        buider =>
        {
            buider.WithOrigins(
                "https://globird.tech",
                "http://localhost:3000"
                )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
        });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddGoogle(options =>
        {
            options.ClientId = "353487917217-3vnjd906comvj5fukp4djf1l3at13mr0.apps.googleusercontent.com";
            options.ClientSecret = "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";
        });
builder.Services.Configure<MailSetting>(configuration.GetSection("MailSettings"));
//builder.Services.AddDbService(configuration);
builder.Services.AddDbLocalService();

//register Repository
builder.Services.AddRepositories();
//register Application Service
builder.Services.AddAppServices();
builder.Services.AddJwtService(configuration);

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GloBird API",
        Version = "v1",
        Description = "API for GloBird Project",
        Contact = new OpenApiContact
        {
            Name = "Contact Developers",
            Url = new Uri("https://github.com/Hai-Ba-Con-Ga")
        }
    });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //options.RoutePrefix = string.Empty;
    });
    await app.Services.DbInitializer();
}
app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = false, ShowIsErrorFlagForSuccessfulResponse = true, WrapWhenApiPathStartsWith = "/server" });
app.UseCors("BirdAllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
