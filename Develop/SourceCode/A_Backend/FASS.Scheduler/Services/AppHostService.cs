using Common.AspNetCore.Extensions;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Frame.Services.Cache.Interfaces;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Boot.Services;
using FASS.Data.Dtos.Setting;
using FASS.Scheduler.Models;
using FASS.Scheduler.Services.CronTasks;
using FASS.Scheduler.Services.EventBus;
using FASS.Scheduler.Services.Events;
using FASS.Scheduler.Services.Extends;
using FASS.Scheduler.Utility;
using FASS.Service.Dtos.Setting;
using System.Reflection;

namespace FASS.Scheduler.Services
{
    public class AppHostService : IHostedService, IAsyncDisposable
    {
        public static AssemblyName EntryAssembly { get; } = Assembly.GetEntryAssembly()?.GetName() ?? Assembly.GetExecutingAssembly().GetName();

        public ILogger<AppHostService> Logger { get; }
        public AppSettings AppSettings { get; }
        public IServiceProvider ServiceProvider { get; }

        public IBootService BootService { get; }

        public EventService EventService { get; }
        public ExtendService ExtendService { get; }
        public EventBusService EventBusService { get; }
        public CronTaskService CronTaskService { get; }

        public AppHostService(
            ILogger<AppHostService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider,
            IBootService bootService,
            EventService eventService,
            ExtendService extendService,
            EventBusService eventBusService,
            CronTaskService cronTaskService)
        {
            Logger = logger;
            AppSettings = appSettings;
            ServiceProvider = serviceProvider;

            Logger.LogInformation($"[{EntryAssembly.Name} V {EntryAssembly.Version}]");
            Logger.LogInformation($"--------Init--------");

            if (AppSettings.Plugin.EnablePlugin)
            {
                var pluginPath = $"{AppContext.BaseDirectory}{AppSettings.Plugin.PluginDirectory}";
                if (Directory.Exists(pluginPath))
                {
                    PluginHelper.Load(pluginPath);
                }
            }

            BootService = bootService;
            BootService.LockCount = AppSettings.Scheduler.LockCount;
            BootService.ArchiveDueTime = AppSettings.Scheduler.ArchiveDueTime;
            BootService.UpdateDueTime = AppSettings.Scheduler.UpdateDueTime;
            BootService.TrafficDueTime = AppSettings.Scheduler.TrafficDueTime;
            BootService.FlowDueTime = AppSettings.Scheduler.FlowDueTime;
            BootService.CarDueTime = AppSettings.Scheduler.CarDueTime;

            EventService = eventService;
            ExtendService = extendService;
            EventBusService = eventBusService;
            CronTaskService = cronTaskService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"--------Start--------");

            await InitCacheAsync();
            await BootService.BootStartAsync(cancellationToken);
            await EventService.StartAsync(cancellationToken);
            await ExtendService.StartAsync(cancellationToken);
            await EventBusService.StartAsync(cancellationToken);
            await CronTaskService.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"--------Stop--------");

            await BootService.BootStopAsync(cancellationToken);
            await EventService.StopAsync(cancellationToken);
            await ExtendService.StopAsync(cancellationToken);
            await EventBusService.StopAsync(cancellationToken);
            await CronTaskService.StopAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            Logger.LogInformation($"--------Dispose--------");

            await BootService.DisposeAsync();
        }

        private async Task InitCacheAsync()
        {
            var dataService = ServiceProvider.GetScopeService<IDataService>();
            BootService.Configs = (await dataService.GetToListAsync<ConfigDto, ConfigEntity>(CacheKey.Setting.Config, e => e.IsEnable)).ToList();
            BootService.DictItems = (await dataService.GetToListAsync<DictItemDto, DictItemEntity>(CacheKey.Setting.DictItem, e => e.IsEnable)).ToList();
            BootService.ConfigData = await dataService.GetConfigToDtoAsync<ConfigDataDto>(CacheKey.Setting.ConfigData);
            _ = await dataService.GetConfigToDtoAsync<ConfigServiceDto>(CacheKey.Setting.ConfigService);
        }
    }
}