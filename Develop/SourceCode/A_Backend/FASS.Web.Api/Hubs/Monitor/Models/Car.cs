namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Car
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? CurrNodeCode { get; set; }
        public string? Charge { get; set; }
        public string? State { get; set; }

        public bool StopAccept { get; set; }
    }
}
