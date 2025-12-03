using Abp;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Logistics.Logs.Authorization.Roles;
using Logistics.Logs.Authorization.Users;
using Logistics.Logs.Configuration;
using Logistics.Logs.Core;
using Logistics.Logs.Localization;
using Logistics.Logs.MultiTenancy;
using Logistics.Logs.Timing;
using Microsoft.Extensions.Configuration;

namespace Logistics.Logs;

[DependsOn(typeof(AbpZeroCoreModule))]
public class LogsCoreModule : AbpModule
{
    private readonly IConfiguration _configuration;


    public LogsCoreModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;

        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        LogsLocalizationConfigurer.Configure(Configuration.Localization);

        // Enable this line to create a multi-tenant application.
        Configuration.MultiTenancy.IsEnabled = LogsConsts.MultiTenancyEnabled;

        // Configure roles
        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        Configuration.Settings.Providers.Add<AppSettingProvider>();

     //   Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = LogsConsts.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = LogsConsts.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(LogsCoreModule).GetAssembly());

        ConnectDb.Initialize(_configuration.GetConnectionString(LogsConsts.ConnectionStringName));
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
    }
}
