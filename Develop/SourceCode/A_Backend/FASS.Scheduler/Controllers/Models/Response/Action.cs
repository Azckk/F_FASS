namespace FASS.Scheduler.Controllers.Models.Response
{
    public class Action
    {
        public required string Code { get; set; }
        public required string ActionType { get; set; }
        public required string BlockingType { get; set; }
        public required string State { get; set; }
    }
}
