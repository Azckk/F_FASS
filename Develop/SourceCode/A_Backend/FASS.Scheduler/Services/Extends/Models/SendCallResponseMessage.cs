using Common.NETCore.Helpers;
using System.Text;

namespace FASS.Scheduler.Services.Extends.Models
{
    public class SendCallResponseMessage
    {
        /// <summary>
        /// 起始码
        /// </summary>
        public byte Begin { get; set; } = 0xBB;

        /// <summary>
        /// 命令码
        /// </summary>
        public byte Command { get; set; } = 0xB1;

        /// <summary>
        /// 车辆编码
        /// </summary>
        public ushort Car { get; set; } = 0;

        /// <summary>
        /// 库位编号
        /// </summary>
        public byte StorageNo { get; set; }

        /// <summary>
        /// 呼叫模式
        /// </summary>
        public byte CallMode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialNo { get; set; }

        /// <summary>
        /// 叫料状态
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// 预留字段
        /// </summary>
        public byte[] Reserve { get; set; } = new byte[21];

        /// <summary>
        /// 校验位
        /// </summary>
        public byte Check { get; set; }

        /// <summary>
        /// 结束码
        /// </summary>
        public byte End { get; set; } = 0xEE;


        public SendCallResponseMessage SetMessage(
            byte command,
            ushort car,
            byte storageNo,
            byte callMode,
            string materialNo,
            byte state)
        {
            Command = command;
            Car = car;
            StorageNo = storageNo;
            CallMode = callMode;
            MaterialNo = materialNo;
            State = state;
            return this;
        }

        public byte[] GetByteArray()
        {
            var byteArray = new byte[50];
            byteArray[0] = Begin;
            byteArray[1] = Command;
            var car = BitConverter.GetBytes(Car);
            byteArray[2] = car[0];
            byteArray[3] = car[1];
            byteArray[4] = StorageNo;
            byteArray[5] = CallMode;
            var materialArr = Encoding.ASCII.GetBytes(MaterialNo!);
            Array.Copy(materialArr, 0, byteArray, 6, materialArr.Length);
            byteArray[26] = State;
            Array.Copy(Reserve, 0, byteArray, 27, Reserve.Length);
            byteArray[48] = Utility.Utility.XOR(byteArray[1..^2]);
            byteArray[49] = End;
            return byteArray;
        }

        public SendCallResponseMessage GetMessage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length < 50) throw new Exception($"数据长度错误：{Utility.Utility.ByteArrayToHexString(byteArray)}");
            if (byteArray[48] != Utility.Utility.XOR(byteArray[1..^2])) throw new Exception($"数据校验错误：{Utility.Utility.ByteArrayToHexString(byteArray)}");
            Begin = byteArray[0];
            Command = byteArray[1];
            Car = BitConverter.ToUInt16(byteArray[2..4]);
            StorageNo = byteArray[4];
            CallMode = byteArray[5];
            MaterialNo = Encoding.ASCII.GetString(byteArray[6..26]).PadLeft(20, '0');
            State = byteArray[26];
            Reserve = byteArray[27..48];
            Check = byteArray[48];
            End = byteArray[49];
            return this;
        }

    }
}
