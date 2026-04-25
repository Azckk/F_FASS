namespace FASS.Scheduler.Services.Events.Models
{
    public class MdcsTask
    {
        public required string TaskId { get; set; }
        public string? Name { get; set; }
        public string? CarId { get; set; }
        public string? CarName { get; set; }
        public required string SrcSiteId { get; set; }
        public required string DestSiteId { get; set; }
        public int Priority { get; set; } = 0;
        public required string State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime Created { get; set; }
    }
}
