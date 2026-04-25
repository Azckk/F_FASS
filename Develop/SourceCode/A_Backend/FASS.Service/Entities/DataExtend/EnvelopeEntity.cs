using Common.Service.Entities;

namespace FASS.Service.Entities.DataExtend
{
    public class EnvelopeEntity : AuditEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Value { get; set; }

    }
}
