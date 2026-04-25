using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class ContainerDto : AuditDto
    {
        public required string AreaId { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public bool IsLock { get; set; }

        public string? Barcode { get; set; }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public string? AreaCode { get; set; }
    }
}
