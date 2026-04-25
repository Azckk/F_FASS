using Common.Service.Models;

namespace FASS.Service.Models.Custom
{
    public class Demo : AuditModel
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }
    }
}
