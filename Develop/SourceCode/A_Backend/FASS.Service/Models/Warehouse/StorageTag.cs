using Common.Service.Models;
using FASS.Data.Models.Warehouse;

namespace FASS.Service.Models.Warehouse
{
    public class StorageTag : AuditModel
    {
        public required string StorageId { get; set; }
        public required string TagId { get; set; }

        public virtual Storage Storage { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
