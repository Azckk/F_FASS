using Common.Service.Entities;

namespace FASS.Service.Entities.BaseExtend
{
    public class EdgeAttributeEntity : AuditEntity
    {
        public required string EdgeId { get; set; }
        public required string AttributeType { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }

    }
}
