using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs.Dto
{
    public class CompareEntityAuditLogDataRequestDto
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
        /// Loại thực thể bị ảnh hưởng
        /// </summary>
        public string EntityType { get; set; }


        /// <summary>
        /// Sử dụng làm phiên bản hiện tại để so sánh với phiên bản trước
        /// </summary>
        public long CreatationTimeInt { get; set; }


    }
}
