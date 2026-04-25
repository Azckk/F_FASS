namespace FASS.Scheduler.Controllers.Models.Response
{
    public class Node
    {
        public required string Code { get; set; }
        public IEnumerable<Action> Actions { get; set; } = [];
    }
}
