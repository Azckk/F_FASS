using Common.Service.Dtos;

namespace FASS.Service.Dtos.Warehouse
{
    public class StorageTagDto : AuditDto
    {
        public required string StorageId { get; set; }
        public required string TagId { get; set; }

        public string? StorageCode { get; set; }
        public string? TagCode { get; set; }
    }
}
