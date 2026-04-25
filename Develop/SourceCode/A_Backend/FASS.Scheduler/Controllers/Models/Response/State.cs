namespace FASS.Scheduler.Controllers.Models.Response
{
    public class State
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public string? CurrState { get; set; }
        public double Battery { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
        public double Speed { get; set; }
        public string? CurrNodeCode { get; set; }
        public string? StartNodeCode { get; set; }
        public string? EndNodeCode { get; set; }
        public string? CurrEdgeCode { get; set; }
        public string? StartEdgeCode { get; set; }
        public string? EndEdgeCode { get; set; }
        public IEnumerable<Task> Tasks { get; set; } = [];
        public IEnumerable<Action> Actions { get; set; } = [];
        public IEnumerable<Alarm> Alarms { get; set; } = [];
        public IEnumerable<Warn> Warns { get; set; } = [];
    }
}
