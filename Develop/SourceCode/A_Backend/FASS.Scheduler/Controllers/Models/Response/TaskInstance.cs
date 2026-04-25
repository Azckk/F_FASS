namespace FASS.Scheduler.Controllers.Models.Response
{
    public class TaskInstance
    {
        public string? CarId { get; set; }
        public string? TaskTemplateId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? State { get; set; }
    }
}
