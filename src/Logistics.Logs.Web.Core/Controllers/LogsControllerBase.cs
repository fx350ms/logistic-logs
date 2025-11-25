using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Logistics.Logs.Controllers
{
    public abstract class LogsControllerBase : AbpController
    {
        protected LogsControllerBase()
        {
            LocalizationSourceName = LogsConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
