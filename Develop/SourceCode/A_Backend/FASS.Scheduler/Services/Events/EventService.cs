using FASS.Scheduler.Models;

namespace FASS.Scheduler.Services.Events
{
    public class EventService
    {
        public ILogger<EventService> Logger { get; }
        public AppSettings AppSettings { get; }
        public IServiceProvider ServiceProvider { get; }

        public EventCarService EventCarService { get; } = null!;
        public EventExtendService EventExtendService { get; } = null!;

        public EventService(
            ILogger<EventService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider)
        {
            Logger = logger;
            AppSettings = appSettings;
            ServiceProvider = serviceProvider;

            if (AppSettings.Event.EnableCar)
            {
                EventCarService = new EventCarService(this);
            }
            if (AppSettings.Event.EnableExtend)
            {
                EventExtendService = new EventExtendService(this);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (AppSettings.Event.EnableCar)
                {
                    EventCarService.Start();
                }
                if (AppSettings.Event.EnableExtend)
                {
                    EventExtendService.Start();
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
                if (AppSettings.Event.EnableCar)
                {
                    EventCarService.Stop();
                }
                if (AppSettings.Event.EnableExtend)
                {
                    EventExtendService.Stop();
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
