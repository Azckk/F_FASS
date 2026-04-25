namespace FASS.Scheduler.Controllers.Models.Request
{
    public class RobotTask
    {
        public required string StorageCode { get; set; }
        public required string Type { get; set; }
        public string? CallModel { get; set; } = "EmptyOnline";
        public string? CarTypeCode { get; set; } = string.Empty;

    }
}
