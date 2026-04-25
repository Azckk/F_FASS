using Common.Service.Entities;

namespace FASS.Service.Entities.Warehouse
{
    public class PreMaterialEntity : AuditEntity
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? StorageId { get; set; }
        public string? StorageName { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }
        public bool IsLock { get; set; }
    }
}
