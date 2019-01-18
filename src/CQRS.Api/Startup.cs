using CQRS.Api.Infrastructure.Filters;
using CQRS.Api.Infrastructure.IntegrationEventLogContexts;
using CQRS.Infrastructure;
using CQRS.Infrastructure.Idempotency;
using EFCore.Kit.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace CQRS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc(Configuration)
                .AddEventBus(Configuration)
                .AddEntityFrameworkSqlServer(Configuration)
                .AddMediatRConfigure()
                .AddCustomSwagger();
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "CQRS V1");
                });
            app.UseMvc();
        }
    }
    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(HttpGlobalReponseFilter));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.TryAddTransient<IIdentityService, IdentityService>();
            //services.TryAddSingleton<BackgroundInitialization>();
            return services;
        }
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            //var config = new RabbitMQEventBusConfig();
            //configuration.Bind("RabbitMQService", config);
            //string endpoint = config.ServiceName;
            //IConsulClient consul = new ConsulClient(option => option.Address = new Uri(configuration["ConsulUrl"]));
            //services.AddScoped<IConsulClient>(x => new ConsulClient(option => option.Address = new Uri(configuration["ConsulUrl"])));
            //if (config.Type != "LOCAL")
            //{
            //    var response = consul.Agent.Services().Result;
            //    var _services = response.Response.Values.Where(t => t.Service.Equals(config.ServiceName)).ToList();
            //    var r = new Random();
            //    var index = r.Next(_services.Count);
            //    var service = _services.ElementAt(index);
            //    endpoint = $"{service.Address}:{service.Port}";
            //}
            //var ampqURI = $"amqp://{config.UserName}:{config.Password}@{endpoint}/{config.VHost}";
            //services.AddRabbitMQEventBus(ampqURI, eBusOption =>
            //{
            //    eBusOption.ClientProvidedAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            //    eBusOption.EnableRetryOnFailure(true, 5000, TimeSpan.FromSeconds(30));
            //    eBusOption.RetryOnFailure(TimeSpan.FromSeconds(1));
            //});
            return services;
        }
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<CQRSDomainContext>(options =>
                {
                    options.UseSqlServer(configuration["Connections:EventBusDb"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            });
                }, ServiceLifetime.Scoped);
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<IntegrationEventLogContext>(options =>
                {
                    options
                        .UseSqlServer(configuration["Connections:CQRS"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            });
                },
                ServiceLifetime.Scoped);
            services.TryAddScoped(typeof(IRepository<>), typeof(DefaultRepository<>));
            return services;
        }
        public static IServiceCollection AddMediatRConfigure(this IServiceCollection services)
        {
            services.AddMediatR();
            services.TryAddScoped<IRequestManager, RequestManager>();
            services.AddScoped<ServiceFactory>(p => p.GetService);
            return services;
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = " HTTP API",
                    Version = "v1",
                    Description = "TheCore Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });
            });
            return services;
        }
    }
}
