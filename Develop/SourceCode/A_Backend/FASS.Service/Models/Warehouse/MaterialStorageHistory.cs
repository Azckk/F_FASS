using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class MaterialStorageHistory : AuditModel
    {
        public required string MaterialId { get; set; }
        public required string StorageId { get; set; }

        public required string State { get; set; }

        public virtual Material Material { get; set; } = null!;
        public virtual Storage Storage { get; set; } = null!;
    }
}
