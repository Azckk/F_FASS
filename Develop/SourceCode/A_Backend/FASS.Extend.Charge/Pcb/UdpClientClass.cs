using System.Net;
using System.Net.Sockets;


namespace FASS.Extend.Charge.Pcb
{
    public class UdpState
    {
        public UdpClient udpClient = null;
        public IPEndPoint ipEndPoint = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public int counter = 0;
        public bool connected;
    }

    public class AsyncUdpClient
    {
        public static bool messageSent = false;
        // 定义节点
        private IPEndPoint localEP = null;
        private IPEndPoint remoteEP = null;
        // 定义UDP发送和接收
        private UdpClient udpReceive = null;
        private UdpClient udpSend = null;
        private UdpState udpSendState = null;
        private UdpState udpReceiveState = null;
        // 异步状态同步
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        public AsyncUdpClient(string remoteIp, int remotePort, int listenPort)
        {
            try
            {
                // 本机节点
                localEP = new IPEndPoint(IPAddress.Any, listenPort);
                // 远程节点
                remoteEP = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
                // 实例化
                udpReceive = new UdpClient(localEP);
                udpReceive.Client.ReceiveTimeout = 5000;
                udpSend = new UdpClient();
                udpSend.Client.SendTimeout = 5000;

                // 分别实例化udpSendState、udpReceiveState
                udpSendState = new UdpState();
                udpSendState.ipEndPoint = remoteEP;
                udpSendState.udpClient = udpSend;

                udpReceiveState = new UdpState();
                udpReceiveState.ipEndPoint = remoteEP;
                udpReceiveState.udpClient = udpReceive;

                udpSend.Connect(remoteEP);
            }
            catch (Exception)
            {

            }
        }
        // 发送函数
        public void SendMsg(byte[] sendBytes)
        {
            string hexStr = $"{string.Join(" ", sendBytes.Select(d => $"{d:X2}"))}";
            //Console.WriteLine($"Send => {hexStr}");
            if (udpSend.Client.Connected)
            {
                // 调用发送回调函数
                udpSend.BeginSend(sendBytes, sendBytes.Length, new AsyncCallback(SendCallback), udpSendState);
                sendDone.WaitOne();
                udpReceive.BeginReceive(new AsyncCallback(ReceiveCallback), udpReceiveState);
                receiveDone.WaitOne();
            }
            else
            {
                Console.WriteLine($"Send =>连接超时!");
                udpSend.Connect(remoteEP);
            }
        }

        // 发送回调函数
        public void SendCallback(IAsyncResult iar)
        {
            UdpState udpState = iar.AsyncState as UdpState;
            if (iar.IsCompleted)
            {
                //Console.WriteLine("number of bytes sent: {0}", udpState.udpClient.EndSend(iar));
                sendDone.Set();
            }
        }

        // 接收回调函数
        public void ReceiveCallback(IAsyncResult iar)
        {
            UdpState udpState = iar.AsyncState as UdpState;
            if (iar.IsCompleted)
            {
                byte[] receiveBytes = udpState.udpClient.EndReceive(iar, ref udpReceiveState.ipEndPoint);
                string hexStr = $"{string.Join(" ", receiveBytes.Select(d => $"{d:X2}"))}";
                //Console.WriteLine($"Recv => {hexStr}");
                receiveDone.Set();
                MyEventArgs e = new MyEventArgs(udpState.ipEndPoint.Address.ToString(), receiveBytes);
                _MsgChange(e);
            }
        }

        //收到的消息
        public delegate void MsgChange(MyEventArgs args);
        public event MsgChange OnMsgChange;
        protected virtual void _MsgChange(MyEventArgs args)
        {
            if (OnMsgChange != null)
            {
                OnMsgChange(args);
            }
        }

        public class MyEventArgs : EventArgs
        {
            public string IP;
            public byte[] Msg;
            public MyEventArgs(string iP, byte[] msg)
            {
                IP = iP;
                Msg = msg;
            }
        }

    }
}
