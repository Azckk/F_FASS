using Common.Service.Models;

namespace FASS.Data.Models.Warehouse
{
    public class Work : AuditModel
    {
        public required string ContainerId { get; set; }
        public string? AreaId { get; set; }

        public string? TaskId { get; set; }
        public string? TaskCode { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public required string State { get; set; }

        public virtual Container Container { get; set; } = null!;
    }
}
