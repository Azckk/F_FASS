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
    [Tags("数据管理-车辆类型")]
    public class CarTypeController : BaseController
    {
        private readonly ILogger<CarTypeController> _logger;
        private readonly ICarTypeService _carTypeService;

        public CarTypeController(
            ILogger<CarTypeController> logger,
            ICarTypeService carService)
        {
            _logger = logger;
            _carTypeService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carTypeService.ToPageAsync(param);
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
            var data = await _carTypeService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarTypeDto carTypeDto)
        {
            await _carTypeService.AddOrUpdateAsync(keyValue, carTypeDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _carTypeService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _carTypeService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _carTypeService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _carTypeService.Set().Where(e => e.IsEnable).OrderBy(e => e.SortNumber).ToListAsync();
            return Ok(data);
        }
    }
}