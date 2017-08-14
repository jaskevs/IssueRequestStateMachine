using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Nancy.Owin;
using Nancy;
using Nancy.Configuration;
using Nancy.TinyIoc;

namespace IssueRequestHost
{
    public class Startup
    {
        IConfiguration Configuration { get; }
        IServiceProvider Provider { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                IBus bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    IRabbitMqHost host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                });

                return bus;
            });

            Provider = services.BuildServiceProvider();
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseOwin(x => x.UseNancy(options =>
            {
                options.Bootstrapper = new Bootstrapper(Provider);
            }));
        }

        public class Bootstrapper : DefaultNancyBootstrapper
        {
            readonly IServiceProvider _serviceProvider;

            public Bootstrapper(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public override void Configure(INancyEnvironment environment)
            {
                environment.Tracing(true, true);
            }

            protected override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                base.ConfigureApplicationContainer(container);
                container.Register(_serviceProvider.GetService<ILoggerFactory>());
                container.Register(_serviceProvider.GetService<IBus>());
            }
        }
    }
}
