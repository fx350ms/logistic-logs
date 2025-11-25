using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Logistics.Logs.AuditLogs.Dto;

namespace Logistics.Logs.Roles;

public interface IEntityAuditLogAppService : IAsyncCrudAppService<EntityAuditLogDto, long, PagedResultRequestDto, EntityAuditLogDto, EntityAuditLogDto>
{
    
}
