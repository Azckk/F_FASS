namespace FASS.Scheduler.Controllers.Models.Request
{
    public class Action
    {
        public required string CarCode { get; set; }
        public required string ActionType { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; } = [];
    }
}