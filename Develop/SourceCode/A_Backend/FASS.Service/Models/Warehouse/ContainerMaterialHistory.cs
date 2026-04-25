using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class ContainerMaterialHistory : AuditModel
    {
        public required string ContainerId { get; set; }
        public required string MaterialId { get; set; }

        public required string State { get; set; }

        public virtual Container Container { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
    }
}
