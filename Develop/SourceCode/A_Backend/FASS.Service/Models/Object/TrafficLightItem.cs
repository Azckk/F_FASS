using Common.Service.Models;


namespace FASS.Service.Models.Object
{
    public class TrafficLightItem : AuditModel
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public required string TrafficLightId { get; set; }

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

        /// <summary>
        /// 对应安全光栅主表数据
        /// </summary>
        public virtual TrafficLight trafficLight { get; set; } = null!;
    }
}
