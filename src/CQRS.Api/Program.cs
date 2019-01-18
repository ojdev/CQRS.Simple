using CQRS.Api.Infrastructure.IntegrationEventLogContexts;
using CQRS.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CQRS.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                .MigrateDbContext<CQRSDomainContext>((context, services) =>
                {
                }).MigrateDbContext<IntegrationEventLogContext>((context, services) =>
               {
               }).Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
