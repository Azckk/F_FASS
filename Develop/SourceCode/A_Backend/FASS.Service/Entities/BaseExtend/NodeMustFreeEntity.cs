using Common.Service.Entities;

namespace FASS.Service.Entities.BaseExtend
{
    public class NodeMustFreeEntity : AuditEntity
    {
        public required string NodeCode { get; set; }
        public required string MustFreeNodeCode { get; set; }
    }
}
