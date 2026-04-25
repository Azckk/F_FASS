using Common.Service.Entities;


namespace FASS.Service.Entities.Setting
{
    public  class ConfigEnvelopeEntity : AuditEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
