namespace FASS.Extend.Car.Fairyland.Pcb
{
    public class SendActionMessage
    {
        public byte Begin { get; set; } = 0xBB;
        public byte Command { get; set; } = 0xA1;
        public ushort Car { get; set; }
        public ulong ActionId { get; set; }
        public byte[] PlaceHolder { get; set; } = new byte[2];
        public NodeMessage NodeMessage { get; set; } = new NodeMessage();
        public byte[] Reserve { get; set; } = new byte[25];
        public byte Check { get; set; }
        public byte End { get; set; } = 0xEE;

        public SendActionMessage SetMessage(
            ushort car,
            ulong actionId,
            NodeMessage nodeMessage)
        {
            Car = car;
            ActionId = actionId;
            NodeMessage = nodeMessage;
            return this;
        }

        public SendActionMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 100) throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[98] != Utility.XOR(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            Car = BitConverter.ToUInt16(byteArray[2..4]);
            ActionId = BitConverter.ToUInt64(byteArray[4..12]);
            PlaceHolder = byteArray[12..14];
            NodeMessage = new NodeMessage().GetMessage(byteArray[14..39]);
            Reserve = byteArray[39..98];
            Check = byteArray[98];
            End = byteArray[99];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[100];
            byteArray[0] = Begin;
            byteArray[1] = Command;
            var car = BitConverter.GetBytes(Car);
            byteArray[2] = car[0];
            byteArray[3] = car[1];
            var action = BitConverter.GetBytes(ActionId);
            byteArray[4] = action[0];
            byteArray[5] = action[1];
            byteArray[6] = action[2];
            byteArray[7] = action[3];
            byteArray[8] = action[4];
            byteArray[9] = action[5];
            byteArray[10] = action[6];
            byteArray[11] = action[7];
            //预留两个字节PlaceHolder[12..14]
            var nodeMessage = NodeMessage.GetByteArray();
            Array.Copy(nodeMessage, 0, byteArray, 14, nodeMessage.Length);
            Array.Copy(Reserve, 0, byteArray, 39, Reserve.Length);
            //byteArray[98] = Check;
            byteArray[98] = Utility.XOR(byteArray[1..^2]);
            byteArray[99] = End;
            return byteArray;
        }
    }
}
