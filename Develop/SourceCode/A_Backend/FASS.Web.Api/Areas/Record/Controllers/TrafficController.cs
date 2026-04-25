using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Services.RecordExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Record.Controllers
{

    [Route("api/v1/Record/[controller]/[action]")]
    [Tags("业务记录-交管记录")]
    public class TrafficController : BaseController
    {
        private readonly ILogger<TrafficController> _logger;
        private readonly ICarService _carService;
        private readonly ITrafficService _trafficService;

        public TrafficController(
            ILogger<TrafficController> logger,
            ICarService carService,
            ITrafficService trafficService)
        {
            _logger = logger;
            _carService = carService;
            _trafficService = trafficService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _trafficService.ToPageAsync(param);
            for (int i = 0; i < page.Data.Count; i++)
            {
                var datediff = (page.Data[i].EndTime ?? DateTime.Now) - page.Data[i].StartTime;
                page.Data[i].Continue = string.Format(@"{0}小时{1}分{2}秒", Math.Floor(datediff.TotalHours), datediff.Minutes, datediff.Seconds);
            }
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _trafficService.FirstOrDefaultAsync(e => e.Id == keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _trafficService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _trafficService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _trafficService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _trafficService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _trafficService.DeleteAllAsync();
            return Ok();
        }
    }

}
