namespace FASS.Web.Api.Models.Pc
{
    public class CarState
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public bool IsOnline { get; set; }
        public string? CurrState { get; set; }
        public double Battery { get; set; }
        public double ElectricCurrent { get; set; }
        public double Voltage { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
        public double Speed { get; set; }
        public int Load { get; set; }
        public bool StopAccept { get; set; }
        public string? ManualMode { get; set; }
        public string? CurrNodeCode { get; set; }
        public string? StartNodeCode { get; set; }
        public string? EndNodeCode { get; set; }
        public string? CurrEdgeCode { get; set; }
        public string? StartEdgeCode { get; set; }
        public string? EndEdgeCode { get; set; }    
        public List<int> HoldingLocks { get; set; } = [];
        public List<int> PendingLocks { get; set; } = [];
        public List<BlockedByItem> BlockedBy { get; set; } = [];
        public string? BlockingTime { get; set; }
        public string? TrafficMessage { get; set; }
        public string? InterLockStatu { get; set; }
        public string? Tags { get; set; }
        public int AquiringLock { get; set; }
        public List<Task> Tasks { get; set; } = [];
        public List<Action> Actions { get; set; } = [];
        public List<Alarm> Alarms { get; set; } = [];
        public List<Warn> Warns { get; set; } = [];
    }
}
