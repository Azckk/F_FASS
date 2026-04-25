namespace FASS.Extend.Car.Fairyland.Pc.Models.Request
{
    public class Action
    {
        public required string Code { get; set; }
        public required string ActionType { get; set; }
        public required string BlockingType { get; set; }
        public List<Parameter> Parameters { get; set; } = [];
    }
}
