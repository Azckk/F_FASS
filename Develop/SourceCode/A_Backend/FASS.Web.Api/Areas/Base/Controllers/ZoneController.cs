using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Base;
using FASS.Data.Services.Base.Interfaces;
using FASS.Service.Services.BaseExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Base.Controllers
{
    [Route("api/v1/Base/[controller]/[action]")]
    [Tags("基础管理-区域管理")]
    public class ZoneController : BaseController
    {
        private readonly ILogger<ZoneController> _logger;
        private readonly IZoneService _zoneService;
        private readonly IMapExtendService _mapExtendService;

        public ZoneController(
            ILogger<ZoneController> logger,
            IZoneService zoneService,
            IMapExtendService mapExtendService)
        {
            _logger = logger;
            _zoneService = zoneService;
            _mapExtendService = mapExtendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _zoneService.ToPageAsync(param);
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
            var data = await _zoneService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] ZoneDto zoneDto)
        {
            await _zoneService.AddOrUpdateAsync(keyValue, zoneDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _zoneService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _zoneService.Set().Where(e => e.IsEnable).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Node()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> NodeGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _zoneService.NodeToPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributes([FromQuery] string keyValue)
        {
            var zone = Guard.NotNull(await _zoneService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var attributes = await _mapExtendService.GetZoneAttributes(zone);
            var data = attributes.Select(e =>
            {
                var extend = new FASS.Data.Models.Map.Extend
                {
                    Key = e.AttributeType,
                    Value = e.Value
                };
                return extend;
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlanRules([FromQuery] string keyValue)
        {
            var zone = Guard.NotNull(await _zoneService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var planRules = await _mapExtendService.GetZonePlanRules(zone);
            return Ok(planRules.ToList());
        }

    }
}