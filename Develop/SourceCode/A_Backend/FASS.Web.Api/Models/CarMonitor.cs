using FASS.Web.Api.Models.Pc;

namespace FASS.Web.Api.Models
{
    public class CarMonitor
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? IpAddress { get; set; }
        public int Port { get; set; }
        public string? CarTypeName { get; set; }
        public double Battery { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
        public double Speed { get; set; }
        public string? CurrState { get; set; }
        public string? CurrNodeCode { get; set; }
        public string? NextNodeCode { get; set; }
        public string? CurrentTaskId { get; set; }
        public bool StopAccept { get; set; }
        public string? ManualMode { get; set; }
        //public List<Alarm> Alarms { get; set; } = [];
        public bool IsAlarm { get; set; } = false;
        public string? AlarmStatu { get; set; }
        public string? TrafficMessage { get; set; }
        public string? InterLockStatu { get; set; }
        public string? Tags { get; set; }
        public List<BlockedByItem> BlockedBy { get; set; } = [];
        public bool IsBlockedBy { get; set; } = false;
        public bool IsOnline { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}
