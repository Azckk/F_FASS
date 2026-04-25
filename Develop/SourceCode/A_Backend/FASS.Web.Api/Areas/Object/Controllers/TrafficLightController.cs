using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.Object;
using FASS.Service.Services.Object;
using FASS.Service.Services.Object.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Object.Controllers
{
    [Route("api/v1/Object/[controller]/[action]")]
    [Tags("数据管理-红绿灯")]
    public class TrafficLightController : BaseController
    {
        private readonly ILogger<TrafficLightController> _logger;

        private readonly ITrafficLightService _trafficLightService;

        private readonly AppSettings _appSettings;

        public TrafficLightController(
            ILogger<TrafficLightController> logger, ITrafficLightService trafficLightService,
           AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _trafficLightService = trafficLightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _trafficLightService.ToPageAsync(param);

            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPageSelect([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _trafficLightService.ToPageAsync(param);
            var fileterDate = page.Data.Where(e => e.IsEnable).ToList();
            var data = new
            {
                rows = fileterDate,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _trafficLightService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TrafficLightDto trafficLightDto)
        {
            var result = await _trafficLightService.AddOrUpdateAsync(keyValue, trafficLightDto);
      
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _trafficLightService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
           
            var data = await _trafficLightService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

     

        [HttpGet]
        public async Task<IActionResult> ItemsGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _trafficLightService.ItemsGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _trafficLightService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _trafficLightService.DisableAsync(keyValues);
            return Ok();
        }
    }
}
