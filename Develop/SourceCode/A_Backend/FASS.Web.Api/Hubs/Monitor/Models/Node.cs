namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Node
    {
        public required string Id { get; set; }
        public required string Code { get; set; }
        public string? Type { get; set; }
    }
}
