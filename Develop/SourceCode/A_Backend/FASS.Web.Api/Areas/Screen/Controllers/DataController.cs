using Common.NETCore;
using Common.NETCore.Extensions;
using FASS.Data.Models.Data;
using FASS.Service.Models.Report;
using FASS.Service.Services.Screen.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Areas.Screen.Controllers
{
    [Route("api/v1/Screen/[controller]/[action]")]
    [Tags("大屏管理-数据展示")]
    public class DataController : BaseController
    {
        private readonly ILogger<DataController> _logger;
        private readonly IDataService _dataService;

        public DataController(
            ILogger<DataController> logger,
            IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        //public override IActionResult Index()
        //{
        //    ViewBag.ServiceApi = "/Screen/DataHub";
        //    return base.Index();
        //}

        /*  [HttpGet]
          public IActionResult GetData()
          {
              var data = new
              {
                  data1 = _dataService.getData1(),
                  data2 = _dataService.getData2(),
                  data3 = _dataService.getData3(),
                  data4 = _dataService.getData4(),
                  data5 = _dataService.getData5(),
                  data6 = _dataService.getData6()
              };
              return Ok(data);
          }*/

        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] string param)
        {
            RequestParam request = Guard.NotNull(param.JsonTo<RequestParam>());
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add("createAtStart", request.CreateAtStart);
            where.Add("createAtEnd", request.CreateAtEnd);
            var tasks = await _dataService.GetTaskCompletionRateAsync(where);
            var state = await _dataService.GetCarStateAsync();
            var charge = await _dataService.GetChargeStateAsync();
            var data = new
            {
                rates = tasks.Item1,
                taskCount = tasks.Item2,
                alarm = await _dataService.GetCurrentDayAlarmAsync(),
                carState = state.Item1,
                carOnline=state.Item2,
                chargeState = charge.Item1,
                cahrgeOnline=charge.Item2,
                storage = await _dataService.GetStorageAsync()
            };
            return Ok(data);
        }
    }
}