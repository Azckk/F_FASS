namespace FASS.Scheduler.Controllers.Models.Response
{
    public class CarInstantAction
    {
        public string? CarId { get; set; }
        public required string ActionType { get; set; }
        public required string BlockingType { get; set; }
        public required string State { get; set; }
        public string? Remark { get; set; }
    }
}
