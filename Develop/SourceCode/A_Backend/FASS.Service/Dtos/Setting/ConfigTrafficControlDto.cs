

using Common.Service.Dtos;

namespace FASS.Service.Dtos.Setting
{
    public class ConfigTrafficControlDto : AuditDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
