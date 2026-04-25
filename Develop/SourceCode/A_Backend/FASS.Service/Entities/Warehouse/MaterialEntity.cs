using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class MaterialEntity : AuditEntity
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

        public virtual ICollection<ContainerMaterialEntity> ContainerMaterials { get; set; } = [];
        public virtual ICollection<MaterialStorageEntity> MaterialStorages { get; set; } = [];

        public virtual ICollection<ContainerMaterialHistoryEntity> ContainerMaterialHistorys { get; set; } = [];
        public virtual ICollection<MaterialStorageHistoryEntity> MaterialStorageHistorys { get; set; } = [];
    }
}
