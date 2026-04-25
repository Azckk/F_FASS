using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class MaterialStorageHistoryDto : AuditDto
    {
        public required string ContainerId { get; set; }
        public required string MaterialId { get; set; }

        public required string State { get; set; }

        public string? MaterialCode { get; set; }
        public string? StorageCode { get; set; }
    }
}
