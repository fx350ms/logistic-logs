using Logistics.Logs.AuditLogs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs.Dto
{
    public class JsonCompareItemDto
    {
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Status { get; set; }  // Same / ValueDifferent / AOnly / BOnly
    }

    public class CompareEntityAuditLogDataResultDto
    {
        public EntityAuditLogDataDto OldVersion { get; set; }
        public EntityAuditLogDataDto NewVersion { get; set; }

        /// <summary>
        /// Danh sách trường giống nhau
        /// </summary>
        public List<JsonCompareItemDto> Same { get; set; } = new();

        /// <summary>
        /// Khác nhau về giá trị
        /// </summary>
        public List<JsonCompareItemDto> Different { get; set; } = new();

        /// <summary>
        /// A có nhưng B không có
        /// </summary>
        public List<JsonCompareItemDto> OldOnly { get; set; } = new();

        /// <summary>
        /// A không có nhưng B có
        /// </summary>
        public List<JsonCompareItemDto> NewOnly { get; set; } = new();
    }
}
