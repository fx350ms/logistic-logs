using Abp.Dependency;
using Abp.Domain.Repositories;
using Logistics.Logs.AuditLogs.Dto;
using Logistics.Logs.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs
{
    public class EntityAuditLogConsumer : IConsumer<EntityAuditLogDto>
    {
        //  private readonly IRepository<EntityAuditLog, long> _repository;
        private readonly IEntityAuditLogAppService _entityLogAppService;

        public EntityAuditLogConsumer(IEntityAuditLogAppService entityLogAppService)
        {
            //_repository = repository;
            _entityLogAppService = entityLogAppService;
        }

        public async Task Consume(ConsumeContext<EntityAuditLogDto> context)
        {
            var message = context.Message;
            await _entityLogAppService.CreateAsync(message);
            
            await Task.CompletedTask;
        }
    }
}
