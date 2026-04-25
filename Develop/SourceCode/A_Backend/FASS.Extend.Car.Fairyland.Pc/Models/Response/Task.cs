namespace FASS.Extend.Car.Fairyland.Pc.Models.Response
{
    public class Task
    {
        public required string Code { get; set; }
        public string? State { get; set; }
        public List<Node> Nodes { get; set; } = [];
    }
}
