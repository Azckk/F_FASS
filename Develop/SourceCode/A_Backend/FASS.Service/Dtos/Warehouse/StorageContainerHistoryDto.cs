using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class StorageContainerHistoryDto : AuditDto
    {
        public required string StorageId { get; set; }
        public required string ContainerId { get; set; }

        public required string State { get; set; }

        public string? StorageCode { get; set; }
        public string? ContainerCode { get; set; }
    }
}
