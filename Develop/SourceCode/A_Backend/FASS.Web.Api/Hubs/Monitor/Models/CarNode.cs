namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class CarNode
    {
        public required string Id { get; set; }
        public string? Type { get; set; }
        public double Process { get; set; }
    }
}
