using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Weather.Configuration.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return host;
        }

        public static IServiceCollection AddDbContext<T>(this IServiceCollection services, IConfiguration configuration)
            where T : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            return services.AddDbContext<T>((services, builder) => builder.UseNpgsql(connectionString));
        }
    }
}
