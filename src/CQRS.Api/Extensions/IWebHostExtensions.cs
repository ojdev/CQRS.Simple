using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Api
{
    public static class IWebHostExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="webHost"></param>
        /// <param name="seeder"></param>
        /// <returns></returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
                using (TContext context = services.GetService<TContext>())
                {
                    try
                    {
                        logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
                        context.Database.Migrate();
                        seeder(context, services);
                        logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
                    }
                }
            }
            return webHost;
        }
    }
}
