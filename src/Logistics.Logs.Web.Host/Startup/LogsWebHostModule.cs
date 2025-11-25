using Abp.Modules;
using Abp.Reflection.Extensions;
using Logistics.Logs.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
    }
}
