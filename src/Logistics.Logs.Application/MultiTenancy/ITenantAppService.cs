using Abp.Application.Services;
using Logistics.Logs.MultiTenancy.Dto;

namespace Logistics.Logs.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

