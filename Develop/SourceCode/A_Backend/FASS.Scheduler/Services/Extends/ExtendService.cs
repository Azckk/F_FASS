using FASS.Scheduler.Models;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendService
    {
        public ILogger<ExtendService> Logger { get; }
        public AppSettings AppSettings { get; }
        public IServiceProvider ServiceProvider { get; }

        public ExtendComClientService ExtendComClientService { get; } = null!;
        public ExtendHttpClientService ExtendHttpClientService { get; } = null!;
        public ExtendHttpServerService ExtendHttpServerService { get; } = null!;
        public ExtendTcpClientService ExtendTcpClientService { get; } = null!;
        public ExtendTcpServerService ExtendTcpServerService { get; } = null!;
        public ExtendUdpServerService ExtendUdpClientService { get; } = null!;
        public ExtendMdcsTaskService ExtendMdcsTaskService { get; } = null!;
        public ExtendStandardAlarmService ExtendStandardAlarmService { get; } = null!;
        public ExtendEmptyBucketFillingService ExtendEmptyBucketFillingService { get; } = null!;

        public ExtendService(
            ILogger<ExtendService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider)
        {
            Logger = logger;
            AppSettings = appSettings;
            ServiceProvider = serviceProvider;

            if (AppSettings.Extend.EnableComClient)
            {
                ExtendComClientService = new ExtendComClientService(this);
            }
            if (AppSettings.Extend.EnableTcpClient)
            {
                ExtendTcpClientService = new ExtendTcpClientService(this);
            }
            if (AppSettings.Extend.EnableTcpServer)
            {
                ExtendTcpServerService = new ExtendTcpServerService(this);
            }
            if (AppSettings.Extend.EnableUdpServer)
            {
                ExtendUdpClientService = new ExtendUdpServerService(this);
            }
            if (AppSettings.Extend.EnableHttpClient)
            {
                ExtendHttpClientService = new ExtendHttpClientService(this);
            }
            if (AppSettings.Extend.EnableHttpServer)
            {
                ExtendHttpServerService = new ExtendHttpServerService(this);
            }
            if (AppSettings.Extend.EnableMdcsTaskSync)
            {
                ExtendMdcsTaskService = new ExtendMdcsTaskService(this);
            }
            if (AppSettings.Extend.EnableStandardAlarmServer)
            {
                ExtendStandardAlarmService = new ExtendStandardAlarmService(this);
            }
            if (AppSettings.Extend.EnableEmptyBucketFilling)
            {
                ExtendEmptyBucketFillingService = new ExtendEmptyBucketFillingService(this);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (AppSettings.Extend.EnableComClient)
                {
                    ExtendComClientService.Start();
                }
                if (AppSettings.Extend.EnableTcpClient)
                {
                    ExtendTcpClientService.Start();
                }
                if (AppSettings.Extend.EnableTcpServer)
                {
                    ExtendTcpServerService.Start();
                }
                if (AppSettings.Extend.EnableUdpServer)
                {
                    ExtendUdpClientService.Start();
                }
                if (AppSettings.Extend.EnableHttpClient)
                {
                    ExtendHttpClientService.Start();
                }
                if (AppSettings.Extend.EnableHttpServer)
                {
                    ExtendHttpServerService.Start();
                }
                if (AppSettings.Extend.EnableMdcsTaskSync)
                {
                    ExtendMdcsTaskService.Start();
                }
                if (AppSettings.Extend.EnableStandardAlarmServer)
                {
                    ExtendStandardAlarmService.Start();
                }
                if (AppSettings.Extend.EnableEmptyBucketFilling)
                {
                    ExtendEmptyBucketFillingService.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (AppSettings.Extend.EnableComClient)
                {
                    ExtendComClientService.Stop();
                }
                if (AppSettings.Extend.EnableTcpClient)
                {
                    ExtendTcpClientService.Stop();
                }
                if (AppSettings.Extend.EnableTcpServer)
                {
                    ExtendTcpServerService.Stop();
                }
                if (AppSettings.Extend.EnableUdpServer)
                {
                    ExtendUdpClientService.Stop();
                }
                if (AppSettings.Extend.EnableHttpClient)
                {
                    ExtendHttpClientService.Stop();
                }
                if (AppSettings.Extend.EnableHttpServer)
                {
                    ExtendHttpServerService.Stop();
                }
                if (AppSettings.Extend.EnableMdcsTaskSync)
                {
                    ExtendMdcsTaskService.Stop();
                }
                if (AppSettings.Extend.EnableStandardAlarmServer)
                {
                    ExtendStandardAlarmService.Stop();
                }
                if (AppSettings.Extend.EnableEmptyBucketFilling)
                {
                    ExtendEmptyBucketFillingService.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
            return Task.CompletedTask;
        }
    }
}