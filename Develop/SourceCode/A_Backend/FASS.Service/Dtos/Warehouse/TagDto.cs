using Common.Service.Dtos;

namespace FASS.Service.Dtos.Warehouse
{
    public class TagDto : AuditDto
    {
        public string? Type { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public required string Colour { get; set; }
    }
}
