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
    [Tags("业务记录-日志记录")]
    public class DiaryController : BaseController
    {
        private readonly ILogger<DiaryController> _logger;
        private readonly IDiaryService _diaryService;
        private readonly ICarService _carService;

        public DiaryController(
            ILogger<DiaryController> logger,
            IDiaryService diaryService,
            ICarService carService)
        {
            _logger = logger;
            _diaryService = diaryService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _diaryService.ToPageAsync(param);
            var carDtos = await _carService.Set().ToListAsync();
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
            var data = await _diaryService.FirstOrDefaultAsync(e => e.Id == keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _diaryService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _diaryService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _diaryService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _diaryService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _diaryService.DeleteAllAsync();
            return Ok();
        }
    }
}