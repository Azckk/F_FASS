using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class Container : AuditModel
    {
        public required string AreaId { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public required bool IsLock { get; set; }

        public string? Barcode { get; set; }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public virtual Area Area { get; set; } = null!;

        public virtual List<StorageContainer> StorageContainers { get; set; } = [];
        public virtual List<ContainerMaterial> ContainerMaterials { get; set; } = [];

        public virtual List<StorageContainerHistory> StorageContainerHistorys { get; set; } = [];
        public virtual List<ContainerMaterialHistory> ContainerMaterialHistorys { get; set; } = [];

        public virtual List<Work> Works { get; set; } = [];
    }
}
