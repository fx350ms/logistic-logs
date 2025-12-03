using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Core;
using Logistics.Logs.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs;

public class EntityAuditLogAppService : AsyncCrudAppService<EntityAuditLog, EntityAuditLogDto, long, PagedResultRequestDto, EntityAuditLogDto, EntityAuditLogDto>, IEntityAuditLogAppService
{
    public EntityAuditLogAppService(IRepository<EntityAuditLog, long> repository) 
        : base(repository)
    {
    }

    public override async Task<EntityAuditLogDto> CreateAsync(EntityAuditLogDto input)
    {
        //var id = await  ConnectDb.ExecuteNonQueryAsync<EntityAuditLogDto>( 
        var parameters = new[]
        {
            new SqlParameter("@EntityId", input.EntityId ?? (object)DBNull.Value),
            new SqlParameter("@TenantId", input.TenantId ?? (object)DBNull.Value),
            new SqlParameter("@ServiceName", input.ServiceName ?? (object)DBNull.Value),
            new SqlParameter("@MethodName", input.MethodName ?? (object)DBNull.Value),
            new SqlParameter("@EntityType", input.EntityType ?? (object)DBNull.Value),
            new SqlParameter("@Data", input.Data ?? (object)DBNull.Value),
            new SqlParameter("@UserId", input.UserId ?? (object)DBNull.Value),
            new SqlParameter("@UserName", input.UserName ?? (object)DBNull.Value)
        };

        var result = await ConnectDb.ExecuteScalarAsync(
            "SP_EntityAuditLogs_InsertAndGetId",
            CommandType.StoredProcedure,
            parameters
        ); // object


        input.Id = Convert.ToInt64(result);
        return input;
    }

}
     
