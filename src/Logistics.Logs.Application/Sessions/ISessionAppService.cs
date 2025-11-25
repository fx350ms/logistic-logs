using Abp.Application.Services;
using Logistics.Logs.Sessions.Dto;
using System.Threading.Tasks;

namespace Logistics.Logs.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
