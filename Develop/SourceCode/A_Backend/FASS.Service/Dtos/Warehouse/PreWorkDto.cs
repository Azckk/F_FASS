using Common.Service.Dtos;

namespace FASS.Service.Dtos.Warehouse
{
    public class PreWorkDto : AuditDto
    {
        public string? ContainerId { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string SrcStorageId { get; set; }
        public required string DestStorageId { get; set; }
        public required string MaterialCode { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }
        public string? SrcStorageName { get; set; }
        public string? DestStorageName { get; set; }
    }
}
