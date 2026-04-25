using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class StorageContainerDto : AuditDto
    {
        public required string StorageId { get; set; }
        public required string ContainerId { get; set; }
    }
}
