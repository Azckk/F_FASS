using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class AreaEntity : AuditEntity
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public virtual ICollection<StorageEntity> Storages { get; set; } = [];

        public virtual ICollection<ContainerEntity> Containers { get; set; } = [];
    }
}
