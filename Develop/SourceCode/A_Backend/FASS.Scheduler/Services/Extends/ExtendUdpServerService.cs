using Common.NETCore;
using Common.NETCore.Helpers;
using System.Net;
using UdpServer = Common.Net.Udp.UdpServer;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendUdpServerService
    {
        public ExtendService ExtendService { get; }

        public bool IsStarted { get; private set; }

        public UdpServer UdpServer { get; private set; } = null!;

        public ExtendUdpServerService(
            ExtendService eventService)
        {
            ExtendService = eventService;

            Init();
        }

        public void Init()
        {
            var udpServerLocalIP = Guard.NotNull(ExtendService.AppSettings.Extend.UdpServerLocalIP);
            var udpServerRemoteIP = Guard.NotNull(ExtendService.AppSettings.Extend.UdpServerRemoteIP);
            UdpServer = new UdpServer()
            {
                LocalEndPoint = IPEndPoint.Parse(udpServerLocalIP),
                RemoteEndPoint = IPEndPoint.Parse(udpServerRemoteIP)
            };
            UdpServer.Started += (server) =>
            {
                Task.Run(() => Keepalive(server));
                ExtendService.Logger.LogInformation($"Started LocalEndPoint:[{server.Server.LocalEndPoint}]");
            };
            UdpServer.Stopped += (server) =>
            {
                Task.Run(() => Reconnect(server));
                ExtendService.Logger.LogInformation($"Stopped");
            };
            UdpServer.Sent += (server, sendByteArray, remote) =>
            {
                ExtendService.Logger.LogInformation($"Sent LocalEndPoint:[{server.Server.LocalEndPoint}] RemoteEndPoint:[{remote}] SendByteArray:[{ByteHelper.ByteArrayToHexString(sendByteArray)}]");
            };
            UdpServer.Received += (server, receiveByteArray, remote) =>
            {
                ExtendService.Logger.LogInformation($"Received LocalEndPoint:[{server.Server.LocalEndPoint}] RemoteEndPoint:[{server.Server.RemoteEndPoint}] ReceiveByteArray:[{ByteHelper.ByteArrayToHexString(receiveByteArray)}]");

                server.Send(receiveByteArray, remote);
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
                UdpServer.StartAndReceive();
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
                UdpServer.Stop();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        private void Keepalive(UdpServer server)
        {
            while (true)
            {
                if (!server.IsStarted)
                {
                    break;
                }
                try
                {
                    byte[] sendByteArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
                    server.Send(sendByteArray);
                }
                catch (Exception ex)
                {
                    ExtendService.Logger.LogInformation(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void Reconnect(UdpServer server)
        {
            while (true)
            {
                if (server.IsStarted)
                {
                    break;
                }
                try
                {
                    server.StartAndReceive();
                }
                catch (Exception ex)
                {
                    ExtendService.Logger.LogInformation(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}