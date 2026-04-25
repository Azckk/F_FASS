using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class MaterialDto : AuditDto
    {
        public required string Code { get; set; }
        public string? Name { get; set; }

        public required string Type { get; set; }
        public required string State { get; set; }

        public bool IsLock { get; set; }

        public string? Barcode { get; set; }

        public string? Batch { get; set; }
        public string? Spec { get; set; }
        public string? Unit { get; set; }

        public int Quantity { get; set; }
    }
}
