
namespace FASS.Service.Consts.FlowExtend
{
    public class TaskRecordConst
    {
        public class State
        {
            public const string Created = "Created";

            public const string Released = "Released";

            public const string Distributed = "Distributed";

            public const string Running = "Running";

            public const string Fetching = "Fetching";

            public const string Putting = "Putting";

            public const string Pausing = "Pausing";

            public const string Resuming = "Resuming";

            public const string Completed = "Completed";

            public const string Canceled = "Canceled";

            public const string Faulted = "Faulted";

            public static readonly string[] Update = new string[1] { "Created" };

            public static readonly string[] Delete = new string[4] { "Created", "Completed", "Canceled", "Faulted" };

            public static readonly string[] Release = new string[1] { "Created" };

            public static readonly string[] Cancel = new string[6] { "Released","Running", "Pausing", "Resuming" , "Fetching", "Putting" };

            public static readonly string[] Pause = new string[1] { "Released" };

            public static readonly string[] Resume = new string[1] { "Pausing" };

            public static readonly string[] Resend = new string[2] { "Canceled", "Completed" };

            public static readonly string[] carTaskType = new string[3] { "Created", "Released", "Running" };

            public static string GetStatusDescription(string state)
            {
                string desc = "未知状态";
                switch (state)
                {
                    case State.Created:
                        desc = "创建";
                        break;
                    case State.Released:
                        desc = "发布中";
                        break;
                    case State.Distributed:
                        desc = "分发中";
                        break;
                    case State.Running:
                        desc = "执行中";
                        break;
                    case State.Fetching:
                        desc = "取货中";
                        break;
                    case State.Putting:
                        desc = "放货中";
                        break;
                    case State.Pausing:
                        desc = "暂停中";
                        break;
                    case State.Resuming:
                        desc = "恢复中";
                        break;
                    case State.Completed:
                        desc = "已完成";
                        break;
                    case State.Canceled:
                        desc = "已取消";
                        break;
                    case State.Faulted:
                        desc = "异常";
                        break;
                    default:
                        break;
                }
                return desc;
            }

        }

    }


}
