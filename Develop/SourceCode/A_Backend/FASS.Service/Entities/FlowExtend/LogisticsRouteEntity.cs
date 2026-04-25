using Common.Service.Entities;
using FASS.Data.Entities.Warehouse;

namespace FASS.Service.Entities.FlowExtend
{
    public class LogisticsRouteEntity : AuditEntity
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string SrcAreaId { get; set; }
        public required string DestAreaId { get; set; }

        public virtual AreaEntity SrcArea { get; set; } = null!;
        public virtual AreaEntity DestArea { get; set; } = null!;

    }
}
