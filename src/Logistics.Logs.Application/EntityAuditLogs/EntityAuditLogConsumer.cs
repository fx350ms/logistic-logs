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
    public class EntityAuditLogConsumer : IConsumer<EntityAuditLogDto>, ITransientDependency
    {
        private readonly IRepository<EntityAuditLog, long> _repository;

        public EntityAuditLogConsumer(IRepository<EntityAuditLog, long> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<EntityAuditLogDto> context)
        {
            var message = context.Message;

            // **Mapping DTO sang Entity**
            var auditLog = new EntityAuditLog
            {
                EntityId = message.EntityId,
                TenantId = message.TenantId,
                ServiceName = message.ServiceName,
                MethodName = message.MethodName,
                EntityType = message.EntityType,
                Data = message.Data,
                CreatationTime = message.CreatationTime,
                UserId = message.UserId,
                UserName = message.UserName
            };

            // **Lưu trữ Entity**
            await _repository.InsertAsync(auditLog);

            // MassTransit tự động xác nhận (Ack) tin nhắn nếu phương thức kết thúc thành công.
            // Nếu có lỗi, nó sẽ thử lại hoặc chuyển tin nhắn vào hàng đợi lỗi (Error Queue).
        }
    }
}
