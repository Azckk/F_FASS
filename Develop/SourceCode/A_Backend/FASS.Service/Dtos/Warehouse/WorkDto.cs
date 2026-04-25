using Common.Service.Dtos;

namespace FASS.Data.Dtos.Warehouse
{
    public class WorkDto : AuditDto
    {
        public required string ContainerId { get; set; }
        public string? AreaId { get; set; }

        public string? TaskId { get; set; }
        public string? TaskCode { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public string? ContainerCode { get; set; }
        public string? AreaName { get; set; }
        public string? SrcStorageName { get; set; }
        public string? DestStorageName { get; set; }
        public string? CarName { get; set; }

    }
}
