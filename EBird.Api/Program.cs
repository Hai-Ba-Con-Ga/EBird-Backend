using AutoWrapper;
using EBird.Api.Configurations;
using EBird.Api.Filters;
using EBird.Application.Hubs;
using EBird.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;

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
         buider
            // .AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
           .WithOrigins(new string[] { "http://localhost:3000", "https://www.globird.tech","http://localhost:5173"
            ,"http://localhost:3001","http://localhost:3002","http://localhost:3003","https://localhost:3000"
            })
          .AllowCredentials();
       });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddSettingService(configuration);
builder.Services.AddDbService(configuration);
// builder.Services.AddDbLocalService();

//register Repository
builder.Services.AddRepositories();
//register Application Service
builder.Services.AddAppServices();
builder.Services.AddJwtService(configuration);
builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = null;
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
             

builder.Services.AddControllers(options =>
{
  options.Filters.Add<ValidateModelStateFilter>();
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
      options.SuppressModelStateInvalidFilter = true;
    });

//builder.Services.AddAutoMapper(typeof(Program).Assembly);
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
  app.UseSwaggerUI(options => options.EnablePersistAuthorization());
  app.UseDeveloperExceptionPage();
  await app.Services.ApplyMigration();
  await app.Services.DbInitializer();
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
  options.RoutePrefix = string.Empty;
});
app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = false, ShowIsErrorFlagForSuccessfulResponse = true, WrapWhenApiPathStartsWith = "/server" });
app.UseCors("BirdAllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapHub<ChatHub>("/hub/chat");
app.MapHub<TestHub>("/hub/test");
// app.MapHub<RequestHub>("/requestHub");


app.Run();
