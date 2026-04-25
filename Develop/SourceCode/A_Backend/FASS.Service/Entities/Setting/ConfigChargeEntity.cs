using Common.Service.Entities;


namespace FASS.Service.Entities.Setting
{
    public class ConfigChargeEntity : AuditEntity
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
   
    }
}
