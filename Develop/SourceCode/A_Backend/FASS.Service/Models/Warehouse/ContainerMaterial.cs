using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class ContainerMaterial : AuditModel
    {
        public required string ContainerId { get; set; }
        public required string MaterialId { get; set; }

        public virtual Container Container { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
    }
}
