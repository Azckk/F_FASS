using FASS.Scheduler.Models;
using FASS.Scheduler.Services.CronTasks.Jobs;
using Quartz;

namespace FASS.Scheduler.Services.CronTasks
{
    public class CronTaskService
    {
        public ILogger<CronTaskService> Logger { get; }
        public AppSettings AppSettings { get; }
        public IServiceProvider ServiceProvider { get; }

        public CronTaskService(
            ILogger<CronTaskService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider)
        {
            Logger = logger;
            AppSettings = appSettings;
            ServiceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (AppSettings.Frame.CronTask.IsEnable)
                {
                    var factory = ServiceProvider.GetRequiredService<ISchedulerFactory>();

                    var scheduler = await factory.GetScheduler();

                    var job = JobBuilder.Create<DefaultJob>()
                        .WithIdentity("defaultJob", "defaultGroup")
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity("defaultTrigger", "defaultGroup")
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(10)
                            .RepeatForever())
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (AppSettings.Frame.CronTask.IsEnable)
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