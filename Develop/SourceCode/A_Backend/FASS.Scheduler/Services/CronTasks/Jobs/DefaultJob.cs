using FASS.Boot.Services;
using Quartz;

namespace FASS.Scheduler.Services.CronTasks.Jobs
{
    public class DefaultJob : IJob
    {
        public ILogger<DefaultJob> Logger { get; }

        public IBootService BootService { get; }

        public DefaultJob(
            ILogger<DefaultJob> logger,
            IBootService bootService)
        {
            Logger = logger;

            BootService = bootService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation($"CurrentTime: {DateTime.Now}");
            Logger.LogInformation($"车辆数量：[{BootService.Cars.Count}]");

            return Task.CompletedTask;
        }
    }
}
