using Common.Service.Models;

namespace FASS.Service.Models.Set
{
    public class ConfigCharge : AuditModel
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
     
    }
}
