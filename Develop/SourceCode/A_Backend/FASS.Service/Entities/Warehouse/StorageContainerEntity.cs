using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class StorageContainerEntity : AuditEntity
    {
        public required string StorageId { get; set; }
        public required string ContainerId { get; set; }

        public virtual StorageEntity Storage { get; set; } = null!;
        public virtual ContainerEntity Container { get; set; } = null!;
    }
}
