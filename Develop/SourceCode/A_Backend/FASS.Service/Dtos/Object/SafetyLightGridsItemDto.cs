using Common.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASS.Service.Dtos.Object
{
    public class SafetyLightGridsItemDto : AuditDto
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public required string SafetyLightGridsId { get; set; }

        /// <summary>
        /// 打开/关闭光栅位置（存储地址位）
        /// </summary>
        public required string OpenCloseSignal { get; set; }


        /// <summary>
        /// 外部信号状态
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 光栅状态
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 对应的工位信息
        /// </summary>
        public required string Station { get; set; }

    }
}
