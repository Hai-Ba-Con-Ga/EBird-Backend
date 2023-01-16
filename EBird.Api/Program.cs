using AutoWrapper;
using EBird.Api.Configurations;
using EBird.Application.AppConfig;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbService(configuration);
builder.Services.AddRepositories();
builder.Services.AddAppServices();
builder.Services.AddJwtService(configuration);

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = false, ShowIsErrorFlagForSuccessfulResponse = true, WrapWhenApiPathStartsWith = "/server" });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
