using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Authorization;
using Logistics.Logs.Entities;

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
            cfg =>
            {
                cfg.AddMaps(thisAssembly);

                cfg.CreateMap<EntityAuditLogDto, EntityAuditLog>().ReverseMap();
            }
        );

    }
}
