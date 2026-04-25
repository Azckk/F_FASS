namespace FASS.Extend.Car.Fairyland.Pc.Models.Request
{
    public class CarAction
    {
        public required string CarCode { get; set; }
        public List<Action> Actions { get; set; } = [];
    }
}
