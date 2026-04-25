using Common.Net.Tcp;
using System.Net;

namespace FASS.Extend.Call
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

        public bool GetButton(string buttonNo)
        {
            var result = false;
            try
            {
                Client.Connect();
                var sendMessage = new SendMessage().SetMessage(byte.Parse(buttonNo));
                var sendByteArray = sendMessage.GetByteArray();
                var receiveByteArray = Client.SendAndReceive(sendByteArray);
                if (receiveByteArray is not null)
                {
                    var receiveMessage = new ReceiveMessage().GetMessage(receiveByteArray);
                    if (receiveMessage is not null && receiveMessage.ButtonStatus > 0)
                    {
                        result = true;
                    }
                    if (receiveByteArray is not null)
                    {
                        if (receiveByteArray.Length == 7 && receiveByteArray[3] > 0)
                        {
                            result = true;
                        }
                        if (receiveByteArray.Length == 11 && receiveByteArray[2] > 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                Client.Disconnect();
            }
            return result;
        }
    }
}