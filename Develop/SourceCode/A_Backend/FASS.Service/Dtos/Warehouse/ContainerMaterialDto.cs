using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class ContainerMaterialDto : AuditDto
    {
        public required string ContainerId { get; set; }
        public required string MaterialId { get; set; }
    }
}
