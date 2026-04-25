namespace FASS.Scheduler.Controllers.Models.Request
{
    public class NodeAction
    {
        public required string NodeCode { get; set; }
        public required string ActionType { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; } = [];
    }
}
