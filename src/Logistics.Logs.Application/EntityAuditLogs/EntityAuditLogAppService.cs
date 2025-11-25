using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Entities;
using Logistics.Logs.Roles;

namespace Logistics.Logs.EntityAuditLogs;

public class EntityAuditLogAppService : AsyncCrudAppService<EntityAuditLog, EntityAuditLogDto, long, PagedResultRequestDto, EntityAuditLogDto, EntityAuditLogDto>, IEntityAuditLogAppService
{
    public EntityAuditLogAppService(IRepository<EntityAuditLog, long> repository) 
        : base(repository)
    {
    }
}

