namespace FASS.Web.Api.Models.Pc
{
    public class Task
    {
        public required string Code { get; set; }
        public string? State { get; set; }
        public List<Node> Nodes { get; set; } = [];
    }
}
