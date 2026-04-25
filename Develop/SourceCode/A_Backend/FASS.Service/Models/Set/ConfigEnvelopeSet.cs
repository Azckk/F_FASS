

namespace FASS.Service.Models.Set
{
    public class ConfigEnvelopeSet
    {
        public bool useEnvelope { get; set; }
        public int EnvelopResolution { get; set; }
        public int EnvelopSz { get; set; }
        public bool EnvelopeSkipSinkPoint { get; set; }
        public int EmptyVehicleLength { get; set; }
        public int EmptyVehicleWidth { get; set; }
        public int LoadedVehicleLength { get; set; }
        public int LoadedVehicleWidth { get; set; }
        public string MapUrl { get; set; }
    }
}
