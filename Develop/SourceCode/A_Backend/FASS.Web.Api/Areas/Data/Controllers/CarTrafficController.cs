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
    [Tags("数据管理-车辆管制")]
    public class CarTrafficController : BaseController
    {
        private readonly ILogger<CarTrafficController> _logger;
        private readonly ICarTrafficService _carTrafficService;
        private readonly ICarService _carService;

        public CarTrafficController(
            ILogger<CarTrafficController> logger,
            ICarTrafficService carTrafficService,
            ICarService carService)
        {
            _logger = logger;
            _carTrafficService = carTrafficService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carTrafficService.ToPageAsync(param);
            var carDtos = await _carService.Set().ToListAsync();
            for (int i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].Remark = $"{carDtos.Where(e => e.Code == page.Data[i].FromCarCode).FirstOrDefault()?.Name},{carDtos.Where(e => e.Code == page.Data[i].ToCarCode).FirstOrDefault()?.Name}";
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
            var data = await _carTrafficService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] CarTrafficDto carTrafficDto)
        {
            await _carTrafficService.AddOrUpdateAsync(keyValue, carTrafficDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _carTrafficService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> FinishEnable([FromBody] List<string> keyValues)
        {
            await _carTrafficService.Repository.ExecuteUpdateAsync(e => keyValues.Contains(e.Id), s => s.SetProperty(b => b.IsFinish, true));
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> FinishDisable([FromBody] List<string> keyValues)
        {
            await _carTrafficService.Repository.ExecuteUpdateAsync(e => keyValues.Contains(e.Id), s => s.SetProperty(b => b.IsFinish, false));
            return Ok();
        }
    }
}