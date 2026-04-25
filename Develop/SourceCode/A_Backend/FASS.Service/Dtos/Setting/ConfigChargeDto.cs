
using Common.Service.Dtos;

namespace FASS.Service.Dtos.Setting
{
    public class ConfigChargeDto : AuditDto
    {
        public required string Key { get; set; }
        public required string Value { get; set; }

    }

}
