using Common.Device.Clients.Modbus;
using System.Net;
namespace FASS.Extend.Door
{
    public class Command
    {
        public ModbusTcpClient Client { get; private set; }
        public Command(IPEndPoint ipEndPoint, int timeout = 500)
        {
            Client = new ModbusTcpClient(ipEndPoint, timeout);
        }

        public byte[]? SendState(string address, byte stationNumber = 1, byte functionCode = 3, ushort readLength = 14)
        {
            var readResult = Client.Read(address, stationNumber, functionCode, readLength);
            if (readResult.IsSucceed)
            {
                var receiveByteArray = readResult.Value;
                return receiveByteArray;
            }
            return null;
        }

        public static ReceiveStateMessage GetReceiveMessage(byte[] byteArray) => new ReceiveStateMessage().GetMessage(byteArray);

        public byte[]? SendControl(ushort command, string address, byte stationNumber = 1, byte functionCode = 5)
        {
            var sendByteArray = Client.GetWriteCommand(address, BitConverter.GetBytes(command), stationNumber, functionCode);
            var writeResult = Client.SendPackageReliable(sendByteArray);
            if (writeResult.IsSucceed)
            {
                var receiveByteArray = writeResult.Value;
                return receiveByteArray;
            }
            return null;
        }

        public byte[]? SendControlOpen(byte stationNumber = 1) => SendControl(65280, "4", stationNumber);//开门

        public byte[]? SendControlClose(byte stationNumber = 1) => SendControl(65280, "5", stationNumber);//关门

        public byte[]? SendControlPause(byte stationNumber = 1) => SendControl(65280, "10", stationNumber);//停止

        public byte[]? SendControlResume(byte stationNumber = 1) => SendControl(0, "10", stationNumber);//解除停止

    }
}
