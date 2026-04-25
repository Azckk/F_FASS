using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class ContainerMaterialEntity : AuditEntity
    {
        public required string ContainerId { get; set; }
        public required string MaterialId { get; set; }

        public virtual ContainerEntity Container { get; set; } = null!;
        public virtual MaterialEntity Material { get; set; } = null!;
    }
}
