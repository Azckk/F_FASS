using Common.AspNetCore.Helpers;
using Common.NETCore;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Instant;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Data.Services.Instant.Interfaces;
using FASS.Data.Services.Record.Interfaces;
using FASS.Service.Models.Report;
using FASS.Service.Services.Report.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Report.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/Report/[controller]/[action]")]
    [Tags("报表统计-报表首页")]

    public class HomeController : BaseController
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<HomeController> _logger;
        private readonly ICarService _carService;
        private readonly IAlarmService _alarmService;
        private readonly IDataService _dataService;
        private readonly ICarInstantActionService _carInstantActionService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly IStorageService _storageService;

        public HomeController(
            IDistributedCache distributedCache,
            ILogger<HomeController> logger,
            ICarService carService,
            IAlarmService alarmService,
             IDataService dataService,
            ICarInstantActionService carInstantActionService,
            ITaskInstanceService taskInstanceService, IStorageService storageService)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _carService = carService;
            _alarmService = alarmService;
            _dataService = dataService;
            _carInstantActionService = carInstantActionService;
            _taskInstanceService = taskInstanceService;
            _storageService = storageService;

        }

        //[TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
        //[TypeFilter(typeof(ActionLogIgonreAttribute))]
        //public override IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> GetIndex()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var data = new
            {
                carAll = await _carService.Set().CountAsync(),
                carRunning = await _carService.Set().CountAsync(e => e.CarState == CarConst.State.Running),
                carInstantActionAll = await _carInstantActionService.Set().CountAsync(),
                carInstantActionCompleted = await _carInstantActionService.Set().CountAsync(e => e.State == CarInstantActionConst.State.Completed),
                taskInstanceAll = await _taskInstanceService.Set().CountAsync(),
                taskInstanceCompleted = await _taskInstanceService.Set().CountAsync(e => e.State == TaskInstanceConst.State.Completed),
                loginName = userIdentity.Name,
                loginTime = userIdentity.Time,
                carInstantActions = await _carInstantActionService.Set().OrderByDescending(e => e.UpdateAt).Take(5).ToListAsync(),
                taskInstances = await _taskInstanceService.Set().OrderByDescending(e => e.UpdateAt).Take(5).ToListAsync(),
                cars = await _carService.Set().OrderByDescending(e => e.UpdateAt).Take(5).ToListAsync(),
                alarms = await _alarmService.Set().OrderByDescending(e => e.CreateAt).Take(5).ToListAsync(),
            };

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetIndexData([FromQuery] string param)
        {
            RequestParam request = Guard.NotNull(param.JsonTo<RequestParam>());
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add("createAtStart", request.CreateAtStart);
            where.Add("createAtEnd", request.CreateAtEnd);
            var tasks = await _dataService.GetTaskCompletionRateAsync(where);
            var data = new
            {
                rates = tasks.Item1,
                taskCount = tasks.Item2,
                alarm = await _dataService.GetCurrentDayAlarmAsync(),
                carState = await _dataService.GetCarStateAsync(),
                chargeState = await _dataService.GetChargeStateAsync(),
                storage = await _dataService.GetStorageAsync()
            };
            return Ok(data);
        }

    }
}