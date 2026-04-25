using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class MaterialStorageEntity : AuditEntity
    {
        public required string MaterialId { get; set; }
        public required string StorageId { get; set; }

        public virtual MaterialEntity Material { get; set; } = null!;
        public virtual StorageEntity Storage { get; set; } = null!;
    }
}
