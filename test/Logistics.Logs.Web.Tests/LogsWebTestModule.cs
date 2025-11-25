using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Logistics.Logs.EntityFrameworkCore;
using Logistics.Logs.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Logistics.Logs.Web.Tests;

[DependsOn(
    typeof(LogsWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class LogsWebTestModule : AbpModule
{
    public LogsWebTestModule(LogsEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(LogsWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(LogsWebMvcModule).Assembly);
    }
}