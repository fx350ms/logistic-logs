using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Logistics.Logs.Authorization;

namespace Logistics.Logs;

[DependsOn(
    typeof(LogsCoreModule),
    typeof(AbpAutoMapperModule))]
public class LogsApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<LogsAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(LogsApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
