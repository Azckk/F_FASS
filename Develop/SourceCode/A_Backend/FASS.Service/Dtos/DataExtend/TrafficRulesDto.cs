using Common.Service.Dtos;

namespace FASS.Service.Dtos.DataExtend
{
    public class TrafficRulesDto : AuditDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Value { get; set; }

    }
}
