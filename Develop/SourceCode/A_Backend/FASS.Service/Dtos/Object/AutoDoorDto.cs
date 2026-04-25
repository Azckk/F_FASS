using Common.Service.Dtos;

namespace FASS.Service.Dtos.Object
{
    /// <summary>
    /// 自动门信息类
    /// </summary>
    public class AutoDoorDto : AuditDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 前站点id
        /// </summary>
        public required string PrevNodeId { get; set; }

        /// <summary>
        /// 后站点id
        /// </summary>
        public required string NextNodeId { get; set; }

        /// <summary>
        /// 是否提前开门
        /// </summary>
        public bool IsAdvance { get; set; }

        /// <summary>
        /// 自动门状态
        /// </summary>
        public required string State { get; set; }
    }


}
