using System.Buffers;

namespace FASS.Extend.Car.Fairyland.Plc
{
    public class SendNodesMessage
    {
        public byte Begin { get; set; } = 0xBB;
        public byte Command { get; set; } = 0xB1;
        public ushort Car { get; set; }
        public ulong Task { get; set; }
        public ushort Count { get; set; }
        public NodeMessage[] NodeMessages { get; set; } = new NodeMessage[10]
        {
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage(),
            new NodeMessage()
        };
        public byte[] Reserve { get; set; } = new byte[34];
        public byte Check { get; set; }
        public byte End { get; set; } = 0xEE;

        public SendNodesMessage SetMessage(
            ushort car,
            ulong task,
            ushort count,
            NodeMessage[] nodeMessages)
        {
            Car = car;
            Task = task;
            Count = count;
            Array.Copy(nodeMessages, 0, NodeMessages, 0, nodeMessages.Length);
            return this;
        }

        public SendNodesMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 300) throw new Exception($"数据长度错误：{Utility.ByteArrayToHexString(byteArray)}");
            //if (byteArray[298] != Utility.XOR(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            Car = BitConverter.ToUInt16(byteArray[2..4]);
            Task = BitConverter.ToUInt64(byteArray[4..12]);
            Count = BitConverter.ToUInt16(byteArray[12..14]);
            for (var i = 0; i < 10; i++)
            {
                var start = i * 25 + 14;
                var end = start + 25;
                NodeMessages[i] = new NodeMessage().GetMessage(byteArray[start..end]);
            }
            Reserve = byteArray[263..298];
            Check = byteArray[298];
            End = byteArray[299];
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[300];
            byteArray[0] = Begin;
            byteArray[1] = Command;
            var car = BitConverter.GetBytes(Car);
            byteArray[2] = car[0];
            byteArray[3] = car[1];
            var task = BitConverter.GetBytes(Task);
            byteArray[4] = task[0];
            byteArray[5] = task[1];
            byteArray[6] = task[2];
            byteArray[7] = task[3];
            byteArray[8] = task[4];
            byteArray[9] = task[5];
            byteArray[10] = task[6];
            byteArray[11] = task[7];
            var count = BitConverter.GetBytes(Count);
            byteArray[12] = count[0];
            byteArray[13] = count[1];
            var nodeMessages = NodeMessages.SelectMany(e => e.GetByteArray()).ToArray();
            Array.Copy(nodeMessages, 0, byteArray, 14, nodeMessages.Length);
            Array.Copy(Reserve, 0, byteArray, 264, Reserve.Length);
            //byteArray[298] = Check;
            byteArray[298] = Utility.XOR(byteArray[1..^2]);
            byteArray[299] = End;
            return byteArray;
        }
    }
}
