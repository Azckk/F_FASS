using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Data.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-车辆动作")]
    public class CarActionController : BaseController
    {
        private readonly ILogger<CarActionController> _logger;
        private readonly ICarActionService _carActionService;

        public CarActionController(
            ILogger<CarActionController> logger,
            ICarActionService carActionService)
        {
            _logger = logger;
            _carActionService = carActionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carActionService.ToPageAsync(param);
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
            var data = await _carActionService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarActionDto carActionDto)
        {
            await _carActionService.AddOrUpdateAsync(keyValue, carActionDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _carActionService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _carActionService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _carActionService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelectByCarTypeCode([FromQuery] string carTypeCode)
        {
            var data = await _carActionService.Set().Where(e => e.IsEnable).Where(e => e.CarTypeCode == carTypeCode).OrderBy(e => e.SortNumber).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelectByCarId([FromQuery] string carId)
        {
            var data = await _carActionService.GetListToSelectByCarIdAsync(carId);
            return Ok(data);
        }
    }
}