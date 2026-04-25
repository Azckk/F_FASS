using Common.Service.Dtos;

namespace FASS.Service.Dtos.BaseExtend
{
    public class ZoneAttributeDto : AuditDto
    {
        public required string ZoneId { get; set; }
        public required string AttributeType { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
    }
}
