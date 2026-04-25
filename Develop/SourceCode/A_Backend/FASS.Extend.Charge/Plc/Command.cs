using Common.Net.Tcp;
using System.Net;

namespace FASS.Extend.Charge.plc
{
    public class Command
    {
        public TcpClient Client { get; private set; }

        public Command(string ip, string port)
        {
            Client = new TcpClient()
            {
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port)),
                SendTimeout = 500,
                ReceiveTimeout = 500
            };
        }

        public byte[]? SendCharge(byte command, ushort car, float chargeElectric, float chargeVoltage, ushort chargeTimeSpan, ushort soc, float electricCurrent, float voltage)
        {
            var sendMessage = new SendMessage().SetMessage(command, car, chargeElectric, chargeVoltage, chargeTimeSpan, soc, electricCurrent, voltage);
            var sendByteArray = sendMessage.GetByteArray();
            var receiveByteArray = Client.SendAndReceive(sendByteArray);
            return receiveByteArray;
        }

        public static ReceiveMessage GetReceiveMessage(byte[] byteArray) => new ReceiveMessage().GetMessage(byteArray);

    }
}