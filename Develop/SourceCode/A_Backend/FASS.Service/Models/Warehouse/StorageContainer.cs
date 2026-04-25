using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class StorageContainer : AuditModel
    {
        public required string StorageId { get; set; }
        public required string ContainerId { get; set; }

        public virtual Storage Storage { get; set; } = null!;
        public virtual Container Container { get; set; } = null!;
    }
}
