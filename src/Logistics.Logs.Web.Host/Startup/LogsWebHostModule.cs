using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Configuration;
using Logistics.Logs.EntityAuditLogs;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Logistics.Logs.Web.Host.Startup
{
    [DependsOn(
       typeof(LogsWebCoreModule))]
    public class LogsWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LogsWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LogsWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.IocContainer.Register(Component.For<EntityAuditLogConsumer>().LifestyleTransient());
            var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(new Uri("rabbitmq://localhost/"), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                config.ReceiveEndpoint(queueName: "repro-service", endpoint =>
                {
                    endpoint.Handler<EntityAuditLogDto>(async context =>
                    {
                        using (var consumer = IocManager.ResolveAsDisposable<EntityAuditLogConsumer>(typeof(EntityAuditLogConsumer)))
                        {
                            await consumer.Object.Consume(context);
                        }
                    });
                });
            });

            IocManager.IocContainer.Register(Component.For<IBus, IBusControl>().Instance(busControl));

            busControl.Start();
            
        }
    }
}
