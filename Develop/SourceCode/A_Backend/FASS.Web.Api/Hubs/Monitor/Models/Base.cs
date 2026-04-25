namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Base
    {
        public Point Point { get; set; } = new Point();
        public Size Size { get; set; } = new Size();
        public double Rotate { get; set; }
        public IEnumerable<CarEdge> CarEdges { get; set; } = [];
        public IEnumerable<CarNode> CarNodes { get; set; } = [];
        public Status Status { get; set; } = new Status();
    }
}
