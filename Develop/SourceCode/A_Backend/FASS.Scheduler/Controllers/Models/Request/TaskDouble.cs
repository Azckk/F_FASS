namespace FASS.Scheduler.Controllers.Models.Request
{
    public class TaskDouble
    {
        public required string CarCode { get; set; }
        public required string StartNodeCode { get; set; }
        public required string EndNodeCode { get; set; }
    }
}
