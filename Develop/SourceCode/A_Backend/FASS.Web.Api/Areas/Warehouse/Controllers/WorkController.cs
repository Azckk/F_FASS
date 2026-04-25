using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-任务管理")]
    public class WorkController : BaseController
    {
        private readonly ILogger<WorkController> _logger;
        private readonly IWorkService _workService;
        private readonly IAreaService _areaService;

        public WorkController(
            ILogger<WorkController> logger,
            IWorkService workService,
            IAreaService areaService)
        {
            _logger = logger;
            _workService = workService;
            _areaService = areaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _workService.ToPageAsync(param);
            var areaDtos = await _areaService.Set().Where(e => e.IsEnable).ToListAsync();
            for (var i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].AreaName = areaDtos.Where(e => e.Id == page.Data[i].AreaId).FirstOrDefault()?.Name;
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
            var data = await _workService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] WorkDto workDto)
        {
            await _workService.AddOrUpdateAsync(keyValue, workDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _workService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _workService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }
    }
}
