using Common.Service.Models;

namespace FASS.Service.Models.Warehouse
{
    public class PreWork : AuditModel
    {
        public string? ContainerId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string SrcStorageId { get; set; }
        public required string DestStorageId { get; set; }
        public required string MaterialCode { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

    }
}
