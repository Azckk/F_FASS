using FASS.Service.Models.FlowExtend;
namespace FASS.Service.Extends.Flow
{
    public class TaskRecordExtend
    {
        /// <summary>
        /// 物料信息
        /// </summary>
        public string? Material { get; set; }

        /// <summary>
        /// 容器尺寸
        /// </summary>
        public ContainerSize ContainerSize { get; set; } = null!;

        /// <summary>
        /// 关联后续任务id
        /// </summary>
        public string? FollowUpTaskId { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int Priority { get; set; }

    }
}
