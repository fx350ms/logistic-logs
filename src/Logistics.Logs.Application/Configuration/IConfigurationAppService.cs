using Logistics.Logs.Configuration.Dto;
using System.Threading.Tasks;

namespace Logistics.Logs.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
