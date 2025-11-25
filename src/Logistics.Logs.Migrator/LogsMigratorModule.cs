using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Logistics.Logs.Configuration;
using Logistics.Logs.EntityFrameworkCore;
using Logistics.Logs.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace Logistics.Logs.Migrator;

[DependsOn(typeof(LogsEntityFrameworkModule))]
public class LogsMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public LogsMigratorModule(LogsEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(LogsMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            LogsConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(LogsMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
