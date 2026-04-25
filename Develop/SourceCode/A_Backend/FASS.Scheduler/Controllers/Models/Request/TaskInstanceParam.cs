namespace FASS.Scheduler.Controllers.Models.Request
{
    public class TaskInstanceParam
    {
        public required string TaskInstanceId { get; set; }
        public string? CarId { get; set; }
    }
}
