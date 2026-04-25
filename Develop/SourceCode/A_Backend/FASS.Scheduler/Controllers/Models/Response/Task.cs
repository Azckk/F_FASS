namespace FASS.Scheduler.Controllers.Models.Response
{
    public class Task
    {
        public required string Code { get; set; }
        public string? State { get; set; }
        public IEnumerable<Node> Nodes { get; set; } = [];
    }
}
