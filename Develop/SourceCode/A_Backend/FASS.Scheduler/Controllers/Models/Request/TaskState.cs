namespace FASS.Scheduler.Controllers.Models.Request
{
    /// <summary>
    /// 任务状态。
    /// </summary>
    public class TaskState
    {
        public required string MissionId { get; set; }

        public required string CarCode { get; set; }

        public enum MissionStateEnum
        {
            /// <summary>
            /// 创建任务。
            /// </summary>
            Created = 1,

            /// <summary>
            /// 子任务启动。
            /// </summary>
            Started = 2,

            /// <summary>
            /// 子任务完成。
            /// </summary>
            Finished = 3,

            /// <summary>
            /// 子任务失败。
            /// </summary>
            Failed = 4,

            /// <summary>
            /// 子任务的取货完成，发生在Started之后。
            /// </summary>
            Fetched = 5,

            /// <summary>
            /// 子任务的放货完成，发生在Started之后。
            /// </summary>
            Put = 6
        }

        public MissionStateEnum State { get; set; }

        public DateTime TriggerTime { get; set; }
    }
}
