namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Map
    {
        public string? Type { get; set; }
        public Code Code { get; set; } = new Code();
        public Name Name { get; set; } = new Name();
        public Base Base { get; set; } = new Base();
    }
}
