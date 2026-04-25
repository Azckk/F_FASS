using Common.Service.Dtos;

namespace FASS.Service.Dtos.DataExtend
{
    public class AttributeDto : AuditDto
    {
        public string? Kind { get; set; }
        public string? Type { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
    }
}
