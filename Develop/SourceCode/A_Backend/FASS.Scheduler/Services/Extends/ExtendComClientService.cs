using Common.NETCore;
using Common.NETCore.Helpers;
using ComClient = Common.Net.Com.ComClient;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendComClientService
    {
        public ExtendService ExtendService { get; }

        public bool IsStarted { get; private set; }

        public ComClient ComClient { get; private set; } = null!;

        public ExtendComClientService(
            ExtendService extendService)
        {
            ExtendService = extendService;

            Init();
        }

        public void Init()
        {
            var comClientPortName = Guard.NotNull(ExtendService.AppSettings.Extend.ComClientPortName);
            ComClient = new ComClient() { PortName = comClientPortName };
            ComClient.Opened += (client) =>
            {
                Task.Run(() => Keepalive(client));
                ExtendService.Logger.LogInformation($"Opened PortName:[{client.Client.PortName}] BaudRate:[{client.Client.BaudRate}]");
            };
            ComClient.Closed += (client) =>
            {
                Task.Run(() => Reconnect(client));
                ExtendService.Logger.LogInformation($"Closed");
            };
            ComClient.Writed += (client, writeByteArray) =>
            {
                ExtendService.Logger.LogInformation($"Writed PortName:[{client.Client.PortName}] BaudRate:[{client.Client.BaudRate}] WriteByteArray:[{ByteHelper.ByteArrayToHexString(writeByteArray)}]");
            };
            ComClient.Readed += (client, readByteArray) =>
            {
                ExtendService.Logger.LogInformation($"Readed PortName:[{client.Client.PortName}] BaudRate:[{client.Client.BaudRate}] ReadByteArray:[{ByteHelper.ByteArrayToHexString(readByteArray)}]");

                ComClient.Write(readByteArray);
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
                ComClient.OpenAndRead();
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
                ComClient.Close();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        private void Keepalive(ComClient client)
        {
            while (true)
            {
                if (!client.IsOpen)
                {
                    break;
                }
                try
                {
                    byte[] sendByteArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
                    client.Write(sendByteArray);
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

        private void Reconnect(ComClient client)
        {
            while (true)
            {
                if (client.IsOpen)
                {
                    break;
                }
                try
                {
                    client.OpenAndRead();
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