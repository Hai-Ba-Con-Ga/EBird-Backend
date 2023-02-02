using EBird.Application.AppConfig;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;



namespace EBird.Api.Configurations
{
    public static class ConfigureDbService
    {
        public static void AddDbService(this IServiceCollection services, IConfiguration configuration)
        {

            ConnectionOption connectionOption = configuration.GetSection(ConnectionOption.ConnectionStrings).Get<ConnectionOption>();
            if(connectionOption == null)
            {
                Console.WriteLine("Connection option is null");
                return;
            }
            Console.WriteLine(connectionOption.DefaultConnection ?? "Connection string is null");
            services.AddDbContext<ApplicationDbContext>(options =>
                                                       options.UseSqlServer(connectionOption.DefaultConnection,
                                                                            x => x.MigrationsAssembly("EBird.Infrastructure")));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
        
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IWapperRepository, WapperRepository>();
        }
    }
}
