using Common.Service.Entities;

namespace FASS.Data.Entities.Warehouse
{
    public class WorkEntity : AuditEntity
    {
        public required string ContainerId { get; set; }
        public string? AreaId { get; set; }

        public string? TaskId { get; set; }
        public string? TaskCode { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public virtual ContainerEntity Container { get; set; } = null!;
    }
}
