using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class Material : AuditModel
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

        public virtual List<ContainerMaterial> ContainerMaterials { get; set; } = [];
        public virtual List<MaterialStorage> MaterialStorages { get; set; } = [];

        public virtual List<ContainerMaterialHistory> ContainerMaterialHistorys { get; set; } = [];
        public virtual List<MaterialStorageHistory> MaterialStorageHistorys { get; set; } = [];
    }
}
