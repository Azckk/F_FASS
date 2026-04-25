namespace FASS.Extend.Car.Fairyland.Plc
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

        public NodeMessage SetMessage(
            ushort node,
            ushort distance,
            byte startStop,
            byte direction,
            byte orientation,
            byte byroad,
            ushort speed,
            byte obstacle,
            byte audio,
            byte light,
            byte charge,
            byte rest,
            byte lift,
            byte clamp,
            byte tray,
            byte roll,
            byte shutdown)
        {
            Node = node;
            Distance = distance;
            StartStop = startStop;
            Direction = direction;
            Orientation = orientation;
            Byroad = byroad;
            Speed = speed;
            Obstacle = obstacle;
            Audio = audio;
            Light = light;
            Charge = charge;
            Rest = rest;
            Lift = lift;
            Clamp = clamp;
            Tray = tray;
            Roll = roll;
            Shutdown = shutdown;
            return this;
        }

        public NodeMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 25) throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");
            Node = BitConverter.ToUInt16(byteArray[0..2]);
            Distance = BitConverter.ToUInt16(byteArray[2..4]);
            StartStop = byteArray[4];
            Direction = byteArray[5];
            Orientation = byteArray[6];
            Byroad = byteArray[7];
            Speed = BitConverter.ToUInt16(byteArray[8..10]);
            Obstacle = byteArray[10];
            Audio = byteArray[11];
            Light = byteArray[12];
            Charge = byteArray[13];
            Rest = byteArray[14];
            Lift = byteArray[15];
            Clamp = byteArray[16];
            Tray = byteArray[17];
            Roll = byteArray[18];
            Shutdown = byteArray[19];
            Reserve = byteArray[20..];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[25];
            var node = BitConverter.GetBytes(Node);
            byteArray[0] = node[0];
            byteArray[1] = node[1];
            var distance = BitConverter.GetBytes(Distance);
            byteArray[2] = distance[0];
            byteArray[3] = distance[1];
            byteArray[4] = StartStop;
            byteArray[5] = Direction;
            byteArray[6] = Orientation;
            byteArray[7] = Byroad;
            var speed = BitConverter.GetBytes(Speed);
            byteArray[8] = speed[0];
            byteArray[9] = speed[1];
            byteArray[10] = Obstacle;
            byteArray[11] = Audio;
            byteArray[12] = Light;
            byteArray[13] = Charge;
            byteArray[14] = Rest;
            byteArray[15] = Lift;
            byteArray[16] = Clamp;
            byteArray[17] = Tray;
            byteArray[18] = Roll;
            byteArray[19] = Shutdown;
            Array.Copy(Reserve, 0, byteArray, 20, Reserve.Length);
            return byteArray;
        }
    }
}
