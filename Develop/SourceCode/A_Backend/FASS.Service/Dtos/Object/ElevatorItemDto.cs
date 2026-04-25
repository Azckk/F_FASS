using Common.Service.Dtos;

namespace FASS.Service.Dtos.Object
{
    public class ElevatorItemDto : AuditDto
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public required string ElevatorId { get; set; }

        /// <summary>
        ///通讯地址位（存储地址位）
        /// </summary>
        public required string OpenCloseSignal { get; set; }


        /// <summary>
        /// 外部信号状态
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 按钮状态
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 对应的工位信息
        /// </summary>
        public required string Station { get; set; }
    }
}
