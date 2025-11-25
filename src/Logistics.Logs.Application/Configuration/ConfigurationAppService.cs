using Abp.Authorization;
using Abp.Runtime.Session;
using Logistics.Logs.Configuration.Dto;
using System.Threading.Tasks;

namespace Logistics.Logs.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : LogsAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
