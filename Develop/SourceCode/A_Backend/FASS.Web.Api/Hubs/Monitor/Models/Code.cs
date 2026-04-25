namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Code
    {
        public bool Visible { get; set; } = true;
        public double GlobalAlpha { get; set; } = 1.0;
        public string Font { get; set; } = "10px Arial";
        public string StrokeStyle { get; set; } = "#000000";
        public string FillStyle { get; set; } = "#000000";
        public Point PointOffset { get; set; } = new Point();
        public string? Text { get; set; }
    }
}
