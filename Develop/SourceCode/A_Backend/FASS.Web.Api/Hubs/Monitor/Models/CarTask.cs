namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class CarTask
    {
        public required string Code { get; set; }
        public required string State { get; set; }
        public string? CarCode { get; set; }
        public string? CarName { get; set; }
    }
}
