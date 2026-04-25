using Quartz;

namespace FASS.Web.Api.Services.CronTasks.Jobs
{
    public class DefaultJob : IJob
    {
        public ILogger<DefaultJob> Logger { get; }

        public DefaultJob(
            ILogger<DefaultJob> logger)
        {
            Logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation($"CurrentTime: {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}
