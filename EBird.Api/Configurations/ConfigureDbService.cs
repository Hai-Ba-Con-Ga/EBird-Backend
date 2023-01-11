﻿using EBird.Application.AppConfig;
using EBird.Application.Interfaces;
using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EBird.Api.Configurations
{
    public static class ConfigureDbService
    {
        public static void AddDbService (this IServiceCollection services)
        {
            var settings = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(settings.Value.ConnectionStrings.DefaultConnection));
        }
        public static void AddRepositories (this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
        }
    }
}
