using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class ContainerEntity : AuditEntity
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

        public virtual AreaEntity Area { get; set; } = null!;

        public virtual ICollection<StorageContainerEntity> StorageContainers { get; set; } = [];
        public virtual ICollection<ContainerMaterialEntity> ContainerMaterials { get; set; } = [];

        public virtual ICollection<StorageContainerHistoryEntity> StorageContainerHistorys { get; set; } = [];
        public virtual ICollection<ContainerMaterialHistoryEntity> ContainerMaterialHistorys { get; set; } = [];

        public virtual ICollection<WorkEntity> Works { get; set; } = [];
    }
}
