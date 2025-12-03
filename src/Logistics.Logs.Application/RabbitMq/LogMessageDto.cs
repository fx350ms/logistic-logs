using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Logs.EntityAuditLogs.Dto
{
    public class LogMessageDto
    {
        public string Service { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
