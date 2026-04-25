using Common.Service.Models;

namespace FASS.Service.Models.BaseExtend
{
    public class EdgeAttribute : AuditModel
    {
        public required string EdgeId { get; set; }
        public required string AttributeType { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
    }
}
