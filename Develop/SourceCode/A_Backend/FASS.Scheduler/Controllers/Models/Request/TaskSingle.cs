namespace FASS.Scheduler.Controllers.Models.Request
{
    public class TaskSingle
    {
        public required string CarCode { get; set; }
        public required string TargetNodeCode { get; set; }
    }
}
