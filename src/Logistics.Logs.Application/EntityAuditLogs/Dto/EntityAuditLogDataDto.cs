using Abp.Application.Services.Dto;
using System;

namespace Logistics.Logs.AuditLogs.Dto
{
    public class EntityAuditLogDataDto : EntityDto<long>
    {
        
        /// <summary>
        /// Hàm thực thi: Tạo mới, Cập nhật, Xóa, ...
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatationTime { get; set; }

        public long CreatationTimeInt { get; set; }

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
