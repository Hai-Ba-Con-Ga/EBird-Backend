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
            if (connectionOption == null)
            {
                Console.WriteLine("Connection option is null");
                return;
            }
            Console.WriteLine(connectionOption.CloudConnection ?? "Connection string is null");
            services.AddDbContext<ApplicationDbContext>(options =>
                                                       options.UseSqlServer(connectionOption.CloudConnection,
                                                                            x => x.MigrationsAssembly("EBird.Infrastructure")));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IWapperRepository, WapperRepository>();
        }
        public static void AddDbLocalService(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                                                       options.UseSqlite("DataSource=GloBird.db",
                                                                            x => x.MigrationsAssembly("EBird.Infrastructure")));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
        public static async Task DbInitializer(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await DatabaseInitializer.InitializeAsync(dbContext);
        }

    }
}
