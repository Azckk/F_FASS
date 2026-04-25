using Common.NETCore;
using Common.NETCore.Helpers;
using System.Net;
using TcpClient = Common.Net.Tcp.TcpClient;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendTcpClientService
    {
        public ExtendService ExtendService { get; }

        public bool IsStarted { get; private set; }

        public TcpClient TcpClient { get; private set; } = null!;

        public ExtendTcpClientService(
            ExtendService eventService)
        {
            ExtendService = eventService;

            Init();
        }

        public void Init()
        {
            var tcpClientRemoteIP = Guard.NotNull(ExtendService.AppSettings.Extend.TcpClientRemoteIP);
            TcpClient = new TcpClient() { RemoteEndPoint = IPEndPoint.Parse(tcpClientRemoteIP) };
            TcpClient.Connected += (client) =>
            {
                Task.Run(() => Keepalive(client));
                ExtendService.Logger.LogInformation($"Connected LocalEndPoint:[{client.Client.LocalEndPoint}] RemoteEndPoint:[{client.Client.RemoteEndPoint}]");
            };
            TcpClient.Disconnected += (client) =>
            {
                Task.Run(() => Reconnect(client));
                ExtendService.Logger.LogInformation($"Disconnected");
            };
            TcpClient.Sent += (client, sendByteArray) =>
            {
                ExtendService.Logger.LogInformation($"Sent LocalEndPoint:[{client.Client.LocalEndPoint}] RemoteEndPoint:[{client.Client.RemoteEndPoint}] SendByteArray:[{ByteHelper.ByteArrayToHexString(sendByteArray)}]");
            };
            TcpClient.Received += (client, receiveByteArray) =>
            {
                ExtendService.Logger.LogInformation($"Received LocalEndPoint:[{client.Client.LocalEndPoint}] RemoteEndPoint:[{client.Client.RemoteEndPoint}] ReceiveByteArray:[{ByteHelper.ByteArrayToHexString(receiveByteArray)}]");
            };
        }

        public void Start()
        {
            try
            {
                if (IsStarted)
                {
                    return;
                }
                IsStarted = true;
                TcpClient.ConnectAndReceive();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                if (!IsStarted)
                {
                    return;
                }
                IsStarted = false;
                TcpClient.Disconnect();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        private void Keepalive(TcpClient client)
        {
            while (true)
            {
                if (!client.IsConnected)
                {
                    break;
                }
                try
                {
                    byte[] sendByteArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
                    client.Send(sendByteArray);
                }
                catch (Exception ex)
                {
                    ExtendService.Logger.LogError(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void Reconnect(TcpClient client)
        {
            while (true)
            {
                if (client.IsConnected)
                {
                    break;
                }
                try
                {
                    client.ConnectAndReceive();
                }
                catch (Exception ex)
                {
                    ExtendService.Logger.LogError(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}