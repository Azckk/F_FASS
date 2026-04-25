using Common.Service.Entities;

namespace FASS.Service.Entities.BaseExtend
{
    public class NodeAttributeEntity : AuditEntity
    {
        public required string NodeId { get; set; }
        public required string AttributeType { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }

    }
}
