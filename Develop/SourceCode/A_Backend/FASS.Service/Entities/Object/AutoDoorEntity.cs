using Common.Service.Entities;

namespace FASS.Service.Entities.Object
{
    public class AutoDoorEntity : AuditEntity
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
        ///// <summary>
        ///// 前一个站点实体对象
        ///// </summary>
        //public virtual NodeEntity PreNodeEntity { get; set; }

        ///// <summary>
        ///// 后一个实体对象实体类
        ///// </summary>
        //public virtual NodeEntity NextNodeEntity { get; set; }
    }
}
