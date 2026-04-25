namespace FASS.Data.Consts.Warehouse
{
    public class WorkConst
    {
        public class Type
        {
            public const string Normal = "Normal";
        }
        public class State
        {
            public const string Created = "Created";
            public const string Released = "Released";

            public const string Distributed = "Distributed";
            public const string Running = "Running";
            public const string Pausing = "Pausing";
            public const string Paused = "Paused";
            public const string Resuming = "Resuming";
            public const string Resumed = "Resumed";
            public const string Completing = "Completing";
            public const string Canceling = "Canceling";
            public const string Faulting = "Faulting";

            public const string Completed = "Completed";
            public const string Canceled = "Canceled";
            public const string Faulted = "Faulted";

            public static readonly string[] Update = [Created];
            public static readonly string[] Release = [Created];
            public static readonly string[] Delete = [Created, Completed, Canceled, Faulted];

            public static readonly string[] Pause = [Running];
            public static readonly string[] Resume = [Paused];
            public static readonly string[] Cancel = [Running, Paused, Resumed];

            public static readonly string[] Distribute = [Released];

            public static readonly string[] Start = [Created, Released];
            public static readonly string[] Execute = [Distributed, Running, Pausing, Paused, Resuming, Resumed, Completing, Canceling, Faulting];
            public static readonly string[] Stop = [Completed, Canceled, Faulted];
        }
    }
}
