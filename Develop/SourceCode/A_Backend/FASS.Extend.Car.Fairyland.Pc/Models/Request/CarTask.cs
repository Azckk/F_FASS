namespace FASS.Extend.Car.Fairyland.Pc.Models.Request
{
    public class CarTask
    {
        public string? CarCode { get; set; }
        public string? CarType { get; set; }
        public required string TaskCode { get; set; }
        public required string TaskType { get; set; }
        public int Priority { get; set; }    
        public string? Material { get; set; }
        public ContainerSize ContainerSize { get; set; } = null!;//容器尺寸
        public List<Node> Nodes { get; set; } = [];
    }
}
