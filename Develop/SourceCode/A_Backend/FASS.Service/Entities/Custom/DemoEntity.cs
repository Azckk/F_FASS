using Common.Service.Entities;

namespace FASS.Service.Entities.Custom
{
    public class DemoEntity : AuditEntity
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }
    }
}
