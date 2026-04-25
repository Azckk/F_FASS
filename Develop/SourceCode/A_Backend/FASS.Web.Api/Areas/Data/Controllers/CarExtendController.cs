using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Data.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-车辆扩展")]
    public class CarExtendController : BaseController
    {
        private readonly ILogger<CarExtendController> _logger;
        private readonly ICarExtendService _carExtendService;

        public CarExtendController(
            ILogger<CarExtendController> logger,
            ICarExtendService carExtendService)
        {
            _logger = logger;
            _carExtendService = carExtendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string carId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carExtendService.ToPageAsync(carId, param);
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
            var data = await _carExtendService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarExtendDto carExtendDto)
        {
            await _carExtendService.AddOrUpdateAsync(keyValue, carExtendDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _carExtendService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}