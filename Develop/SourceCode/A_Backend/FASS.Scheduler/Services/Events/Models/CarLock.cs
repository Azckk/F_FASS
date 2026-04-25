namespace FASS.Scheduler.Services.Events.Models
{
    public class CarLock
    {
        public required string CarCode { get; set; }
        public bool IsLock { get; set; }
    }
}
