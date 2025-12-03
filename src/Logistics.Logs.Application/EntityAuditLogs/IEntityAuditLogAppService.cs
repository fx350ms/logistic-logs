using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Logistics.Logs.AuditLogs.Dto;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs;

public interface IEntityAuditLogAppService : IAsyncCrudAppService<EntityAuditLogDto, long, PagedResultRequestDto, EntityAuditLogDto, EntityAuditLogDto>
{
   
}
