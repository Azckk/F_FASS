using FASS.Boot.Services;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Core.Services;
using FASS.Domain.Services;
using FASS.Scheduler.Models;
using FASS.Scheduler.Services.CronTasks;
using FASS.Scheduler.Services.EventBus;
using FASS.Scheduler.Services.Events;
using FASS.Scheduler.Services.Extends;
using FASS.Service.Extensions;

namespace FASS.Scheduler.Extensions.Configure
{
    public static class BootExtension
    {
        public static IServiceCollection AddBoot(this IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
        {
            services.AddSingleton<ICoreService, CoreService>();
            services.AddSingleton<IDomainService, DomainService>();
            services.AddSingleton<IBootService, BootService>();

            services.AddService(configuration, appSettings.App.ActivationCode, () => appSettings.Frame);

            services.AddKeyedTransient<ICarSessionService, Services.Cars.Fairyland.Plc.CarSessionService>(KeyedService.AnyKey);
            services.AddKeyedTransient<ICarRequestService, Services.Cars.Fairyland.Plc.CarRequestService>(KeyedService.AnyKey);
            services.AddKeyedTransient<ICarResponseService, Services.Cars.Fairyland.Plc.CarResponseService>(KeyedService.AnyKey);

            services.AddKeyedTransient<ICarSessionService, Services.Cars.Fairyland.Plc.CarSessionService>("Fairyland.Plc");
            services.AddKeyedTransient<ICarRequestService, Services.Cars.Fairyland.Plc.CarRequestService>("Fairyland.Plc");
            services.AddKeyedTransient<ICarResponseService, Services.Cars.Fairyland.Plc.CarResponseService>("Fairyland.Plc");

            services.AddKeyedTransient<ICarSessionService, Services.Cars.Fairyland.Pcb.CarSessionService>("Fairyland.Pcb");
            services.AddKeyedTransient<ICarRequestService, Services.Cars.Fairyland.Pcb.CarRequestService>("Fairyland.Pcb");
            services.AddKeyedTransient<ICarResponseService, Services.Cars.Fairyland.Pcb.CarResponseService>("Fairyland.Pcb");

            services.AddKeyedTransient<ICarSessionService, Services.Cars.Fairyland.Pc.CarSessionService>("Fairyland.Pc");
            services.AddKeyedTransient<ICarRequestService, Services.Cars.Fairyland.Pc.CarRequestService>("Fairyland.Pc");
            services.AddKeyedTransient<ICarResponseService, Services.Cars.Fairyland.Pc.CarResponseService>("Fairyland.Pc");

            services.AddSingleton<EventService>();
            services.AddSingleton<ExtendService>();

            services.AddSingleton<CronTaskService>();
            services.AddSingleton<EventBusService>();

            return services;
        }

        public static IServiceProvider UseBoot(this IServiceProvider provider)
        {
            provider.UseService();

            return provider;
        }
    }
}
