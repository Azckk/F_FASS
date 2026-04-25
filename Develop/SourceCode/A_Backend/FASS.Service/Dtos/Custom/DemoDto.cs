using Common.Service.Dtos;

namespace FASS.Service.Dtos.Custom
{
    public class DemoDto : AuditDto
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

    }
}
