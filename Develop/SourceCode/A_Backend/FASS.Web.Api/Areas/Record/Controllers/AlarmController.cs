using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Record.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Record.Controllers
{
    [Route("api/v1/Record/[controller]/[action]")]
    [Tags("业务记录-报警记录")]
    public class AlarmController : BaseController
    {
        private readonly ILogger<AlarmController> _logger;
        private readonly IAlarmService _alarmService;
        private readonly ICarService _carService;

        public AlarmController(
            ILogger<AlarmController> logger,
            IAlarmService alarmService,
            ICarService carService)
        {
            _logger = logger;
            _carService = carService;
            _alarmService = alarmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var carDtos = await _carService.Set().ToListAsync();
            var page = await _alarmService.ToPageAsync(param);
            for (int i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].Remark = carDtos.Where(e => e.Code == page.Data[i].Code).FirstOrDefault()?.Name;
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
            var data = await _alarmService.FirstOrDefaultAsync(e => e.Id == keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _alarmService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _alarmService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _alarmService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _alarmService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _alarmService.DeleteAllAsync();
            return Ok();
        }
    }
}