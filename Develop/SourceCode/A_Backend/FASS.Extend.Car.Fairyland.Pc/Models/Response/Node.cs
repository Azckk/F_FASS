namespace FASS.Extend.Car.Fairyland.Pc.Models.Response
{
    public class Node
    {
        public required string Code { get; set; }
        public List<Action> Actions { get; set; } = [];
    }
}
