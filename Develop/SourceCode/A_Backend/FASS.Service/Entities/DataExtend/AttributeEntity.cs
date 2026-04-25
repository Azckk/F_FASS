using Common.Service.Entities;

namespace FASS.Service.Entities.DataExtend
{
    public class AttributeEntity : AuditEntity
    {
        public string? Kind { get; set; }
        public string? Type { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }

    }
}
