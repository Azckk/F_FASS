namespace FASS.Service.Consts.Data
{
    public class CarConstExtend
    {
        public class State
        {
            public const string None = "None";

            public const string Running = "Running";

            public const string Stopping = "Stopping";

            public const string EmergencyStop = "EmergencyStop";

            public const string Faulting = "Faulting";

            public const string Tasking = "Tasking";

            public const string Dormancying = "Dormancying";

            public const string Shutdown = "Shutdown";

            public const string Charging = "Charging";

            public const string StopAccept = "StopAccept";

            public static readonly string[] OffLine = new string[1] { "Shutdown" };

            public static readonly string[] Idle = new string[3] { "None", "Stopping", "Dormancying" };

            public static readonly string[] Executing = new string[2] { "Running", "Tasking" };

            public static readonly string[] Malfunction = new string[2] { "EmergencyStop", "Faulting" };

        }

    }
}
