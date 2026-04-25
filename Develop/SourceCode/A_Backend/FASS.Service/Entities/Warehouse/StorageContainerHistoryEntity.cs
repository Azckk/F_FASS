using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class StorageContainerHistoryEntity : AuditEntity
    {
        public required string StorageId { get; set; }
        public required string ContainerId { get; set; }

        public required string State { get; set; }

        public virtual StorageEntity Storage { get; set; } = null!;
        public virtual ContainerEntity Container { get; set; } = null!;
    }
}
