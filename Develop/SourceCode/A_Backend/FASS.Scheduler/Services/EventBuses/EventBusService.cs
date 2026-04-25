using DotNetCore.CAP;
using FASS.Scheduler.Models;
using FASS.Scheduler.Services.EventBus.Subscribes;

namespace FASS.Scheduler.Services.EventBus
{
    public class EventBusService : ICapSubscribe
    {
        public ILogger<EventBusService> Logger { get; }
        public AppSettings AppSettings { get; }
        public IServiceProvider ServiceProvider { get; }

        public DefaultSubscribe DefaultSubscribe { get; } = null!;

        public EventBusService(
            ILogger<EventBusService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider)
        {
            Logger = logger;
            AppSettings = appSettings;
            ServiceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (AppSettings.Frame.EventBus.IsEnable)
                {

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
                if (AppSettings.Frame.EventBus.IsEnable)
                {

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
