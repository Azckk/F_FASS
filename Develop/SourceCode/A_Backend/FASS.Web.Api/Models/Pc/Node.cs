namespace FASS.Web.Api.Models.Pc
{
    public class Node
    {
        public required string Code { get; set; }
        public List<Action> Actions { get; set; } = [];
    }
}
