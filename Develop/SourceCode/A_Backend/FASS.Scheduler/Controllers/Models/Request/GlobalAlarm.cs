namespace FASS.Scheduler.Controllers.Models.Request
{
    public class GlobalAlarm
    {
        public string? Level { get; set; }
        public required string alarmInfo { get; set; }
    }
}
