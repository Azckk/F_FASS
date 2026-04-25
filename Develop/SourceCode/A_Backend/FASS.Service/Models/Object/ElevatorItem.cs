using Common.Service.Models;


namespace FASS.Service.Models.Object
{
    public class ElevatorItem : AuditModel
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public required string ElevatorId { get; set; }

        /// <summary>
        /// 电梯对应的键（存储地址位）
        /// </summary>
        public required string OpenCloseSignal { get; set; }

        /// <summary>
        /// 外部信号状态
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 电梯状态
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 对应的工位信息
        /// </summary>
        public required string Station { get; set; }

        /// <summary>
        /// 电梯主表数据
        /// </summary>
        public virtual Elevator elevator { get; set; } = null!;
    }
}
