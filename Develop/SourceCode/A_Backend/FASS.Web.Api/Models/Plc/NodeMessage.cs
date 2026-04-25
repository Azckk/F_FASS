namespace FASS.Web.Api.Models.Plc
{
    public class NodeMessage
    {
        public ushort Node { get; set; }
        public ushort Distance { get; set; }
        public byte StartStop { get; set; }
        public byte Direction { get; set; }
        public byte Orientation { get; set; }
        public byte Byroad { get; set; }
        public ushort Speed { get; set; }
        public byte Obstacle { get; set; }
        public byte Audio { get; set; }
        public byte Light { get; set; }
        public byte Charge { get; set; }
        public byte Rest { get; set; }
        public byte Lift { get; set; }
        public byte Clamp { get; set; }
        public byte Tray { get; set; }
        public byte Roll { get; set; }
        public byte Shutdown { get; set; }
        public byte[] Reserve { get; set; } = new byte[5];

    }
}
