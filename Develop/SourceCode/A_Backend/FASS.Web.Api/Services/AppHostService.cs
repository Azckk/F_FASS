using Common.AspNetCore.Extensions;
using Common.Frame.Dtos.Frame;
using Common.Frame.Entities.Frame;
using Common.Frame.Services.Cache.Interfaces;
using FASS.Data.Dtos.Setting;
using FASS.Service.Dtos.Setting;
using FASS.Web.Api.Models;
using FASS.Web.Api.Services.CronTasks;
using FASS.Web.Api.Services.EventBus;
using FASS.Web.Api.Utility;
using System.Reflection;

namespace FASS.Web.Api.Services
{
    public class AppHostService : IHostedService, IAsyncDisposable
    {
        public static AssemblyName EntryAssembly { get; } = Assembly.GetEntryAssembly()?.GetName() ?? Assembly.GetExecutingAssembly().GetName();

        public ILogger<AppHostService> Logger { get; }
        public AppSettings AppSettings { get; }
        public IServiceProvider ServiceProvider { get; }

        public EventBusService EventBusService { get; }
        public CronTaskService CronTaskService { get; }

        public AppHostService(
            ILogger<AppHostService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider,
            EventBusService eventBusService,
            CronTaskService cronTaskService)
        {
            Logger = logger;
            AppSettings = appSettings;
            ServiceProvider = serviceProvider;

            Logger.LogInformation($"[{EntryAssembly.Name} V {EntryAssembly.Version}]");
            Logger.LogInformation($"--------Init--------");

            EventBusService = eventBusService;
            CronTaskService = cronTaskService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"--------Start--------");

            await InitCacheAsync();
            await EventBusService.StartAsync(cancellationToken);
            await CronTaskService.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"--------Stop--------");

            await EventBusService.StopAsync(cancellationToken);
            await CronTaskService.StopAsync(cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            Logger.LogInformation($"--------Dispose--------");

            return ValueTask.CompletedTask;
        }

        private async Task InitCacheAsync()
        {
            var dataService = ServiceProvider.GetScopeService<IDataService>();
            await dataService.GetToListAsync<ConfigDto, ConfigEntity>(CacheKey.Setting.Config, e => e.IsEnable);
            await dataService.GetToListAsync<DictItemDto, DictItemEntity>(CacheKey.Setting.DictItem, e => e.IsEnable);
            await dataService.GetConfigToDtoAsync<ConfigDataDto>(CacheKey.Setting.ConfigData);
            await dataService.GetConfigToDtoAsync<ConfigServiceDto>(CacheKey.Setting.ConfigService);
        }
    }
}