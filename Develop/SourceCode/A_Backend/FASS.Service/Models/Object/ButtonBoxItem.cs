using Common.Service.Models;


namespace FASS.Service.Models.Object
{
    public class ButtonBoxItem : AuditModel
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public required string ButtonBoxId { get; set; }

        /// <summary>
        /// 按钮对应的键（存储地址位）
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

        /// <summary>
        /// 对按钮盒主表数据
        /// </summary>
        public virtual ButtonBox buttonbox { get; set; } = null!;
    }
}
