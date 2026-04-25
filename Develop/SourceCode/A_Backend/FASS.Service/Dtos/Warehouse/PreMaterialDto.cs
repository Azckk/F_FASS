using Common.Service.Dtos;
namespace FASS.Service.Dtos.Warehouse
{
    public class PreMaterialDto : AuditDto
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
