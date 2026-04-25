using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class AreaDto : AuditDto
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }
    }
}
