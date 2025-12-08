using Castle.Core.Logging;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Core;
using Logistics.Logs.Entities;
using MassTransit;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs
{
    public class EntityAuditLogConsumer : IConsumer<EntityAuditLogDto>
    {
        //  private readonly IRepository<EntityAuditLog, long> _repository;
        private readonly IEntityAuditLogAppService _entityLogAppService;
        private readonly ILogger _logger;
        public EntityAuditLogConsumer(IEntityAuditLogAppService entityLogAppService, ILogger logger)
        {
            //_repository = repository;
            _entityLogAppService = entityLogAppService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EntityAuditLogDto> context)
        {
            var message = context.Message;
            // await _entityLogAppService.CreateAsync(message);
            await LogEntityAuditAsync(message);
            await Task.CompletedTask;
        }

        private async Task LogEntityAuditAsync(EntityAuditLogDto input)
        {
            try
            {
                var parameters = new[]
            {
                new SqlParameter("@EntityId", input.EntityId ?? (object)DBNull.Value),
                new SqlParameter("@TenantId", input.TenantId ?? (object)DBNull.Value),
                new SqlParameter("@ServiceName", input.ServiceName ?? (object)DBNull.Value),
                new SqlParameter("@MethodName", input.MethodName ?? (object)DBNull.Value),
                new SqlParameter("@EntityType", input.EntityType ?? (object)DBNull.Value),
                new SqlParameter("@Title", input.Title ?? (object)DBNull.Value),
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

            }
            catch (Exception ex)
            {
                _logger.Error($"Error logging entity audit: {ex.Message}", ex);
            }


            await Task.CompletedTask;
        }
    }
}
