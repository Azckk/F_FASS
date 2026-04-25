using Common.Service.Entities;

namespace FASS.Service.Entities.Warehouse
{
    public class TagEntity : AuditEntity
    {
        public string? Type { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public required string Colour { get; set; }
        public virtual ICollection<StorageTagEntity> StorageTags { get; set; } = [];
    }

}
