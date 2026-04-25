namespace FASS.Web.Api.Models.Pc
{
    public class DeviceSignal
    {
        public required string DeviceName { get; set; }

        public List<SignalInfo> SignalInfos = [];
    }

    public class SignalInfo
    {
        public required string TypeName { get; set; }
        public required string TypeCode { get; set; }

        public List<Parameter> Signals = [];
    }

}
