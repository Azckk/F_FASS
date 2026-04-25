using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class ContainerMaterialHistoryDto : AuditDto
    {
        public required string ContainerId { get; set; }
        public required string MaterialId { get; set; }

        public required string State { get; set; }

        public string? ContainerCode { get; set; }
        public string? MaterialCode { get; set; }
    }
}
