using Common.Service.Entities;
using FASS.Service.Entities.Warehouse;

namespace FASS.Data.Entities.Warehouse
{
    public class StorageEntity : AuditEntity
    {
        public required string AreaId { get; set; }

        public required string NodeId { get; set; }
        public required string NodeCode { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public required bool IsLock { get; set; }

        public string? Barcode { get; set; }

        public virtual AreaEntity Area { get; set; } = null!;

        public virtual ICollection<StorageContainerEntity> StorageContainers { get; set; } = [];
        public virtual ICollection<MaterialStorageEntity> MaterialStorages { get; set; } = [];

        public virtual ICollection<StorageContainerHistoryEntity> StorageContainerHistorys { get; set; } = [];
        public virtual ICollection<MaterialStorageHistoryEntity> MaterialStorageHistorys { get; set; } = [];
        public virtual ICollection<StorageTagEntity> StorageTags { get; set; } = [];
    }
}
