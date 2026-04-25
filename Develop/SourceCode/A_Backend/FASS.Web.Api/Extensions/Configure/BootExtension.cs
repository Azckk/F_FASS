using FASS.Service.Extensions;
using FASS.Web.Api.Models;
using FASS.Web.Api.Services.CronTasks;
using FASS.Web.Api.Services.EventBus;

namespace FASS.Web.Api.Extensions.Configure
{
    public static class BootExtension
    {
        public static IServiceCollection AddBoot(this IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
        {
            services.AddSingleton<CronTaskService>();
            services.AddSingleton<EventBusService>();

            services.AddService(configuration, appSettings.App.ActivationCode, () => appSettings.Frame);

            return services;
        }

        public static IServiceProvider UseBoot(this IServiceProvider provider)
        {
            provider.UseService();

            return provider;
        }
    }
}
