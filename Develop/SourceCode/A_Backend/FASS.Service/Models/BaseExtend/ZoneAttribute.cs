using Common.Service.Models;

namespace FASS.Service.Models.BaseExtend
{
    public class ZoneAttribute : AuditModel
    {
        public required string ZoneId { get; set; }
        public required string AttributeType { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
    }
}
