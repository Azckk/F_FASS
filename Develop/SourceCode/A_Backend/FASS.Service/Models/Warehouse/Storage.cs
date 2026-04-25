using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class Storage : AuditModel
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

        public virtual Area Area { get; set; } = null!;

        public virtual List<StorageContainer> StorageContainers { get; set; } = [];
        public virtual List<MaterialStorage> MaterialStorages { get; set; } = [];

        public virtual List<StorageContainerHistory> StorageContainerHistorys { get; set; } = [];
        public virtual List<MaterialStorageHistory> MaterialStorageHistorys { get; set; } = [];
    }
}
