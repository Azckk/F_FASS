using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.Object;
using FASS.Service.Services.Object.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Object.Controllers
{
    [Route("api/v1/Object/[controller]/[action]")]
    [Tags("数据管理-红绿灯地址位")]
    public class TrafficLightItemController : BaseController
    {
        private readonly ILogger<TrafficLightItemController> _logger;

        private readonly ITrafficLightItemService _trafficLightItemService;

        private readonly AppSettings _appSettings;

        public TrafficLightItemController(
            ILogger<TrafficLightItemController> logger, ITrafficLightItemService trafficLightItemService,
           AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _trafficLightItemService = trafficLightItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _trafficLightItemService.ToPageAsync(param);

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
            var page = await _trafficLightItemService.ToPageAsync(param);
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
            var data = await _trafficLightItemService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TrafficLightItemDto trafficLightItemDto)
        {
            var result = await _trafficLightItemService.AddOrUpdateAsync(keyValue, trafficLightItemDto);
      
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _trafficLightItemService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
           
            var data = await _trafficLightItemService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

     

     

    }
}
