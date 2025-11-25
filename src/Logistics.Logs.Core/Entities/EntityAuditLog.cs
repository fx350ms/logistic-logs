using Abp.Domain.Entities;
using System;

namespace Logistics.Logs.Entities
{
    public class EntityAuditLog : Entity<long>
    {
        /// <summary>
        /// Id của thực thể bị ảnh hưởng
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Tenant Id của thực thể bị ảnh hưởng
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// Dịch vụ
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Hàm thực thi: Tạo mới, Cập nhật, Xóa, ...
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Loại thực thể bị ảnh hưởng
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatationTime { get; set; }

        /// <summary>
        /// Id người dùng thực hiện
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Tên người dùng thực hiện
        /// </summary>
        public string UserName { get; set; }    

    }
}
