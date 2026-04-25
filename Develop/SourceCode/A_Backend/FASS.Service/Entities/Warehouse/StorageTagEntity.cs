using Common.Service.Entities;
using FASS.Data.Entities.Warehouse;

namespace FASS.Service.Entities.Warehouse
{
    public class StorageTagEntity : AuditEntity
    {
        public required string StorageId { get; set; }
        public required string TagId { get; set; }

        public virtual StorageEntity Storage { get; set; } = null!;
        public virtual TagEntity Tag { get; set; } = null!;
    }
}
