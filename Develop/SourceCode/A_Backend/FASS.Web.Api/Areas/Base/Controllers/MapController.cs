using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Base;
using FASS.Data.Services.Base.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;
using FASS.Web.Api.Utility;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc;

namespace FASS.Web.Api.Areas.Base.Controllers
{
    [Route("api/v1/Base/[controller]/[action]")]
    [Tags("基础管理-地图管理")]
    public class MapController : BaseController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IMapService _mapService;
        private readonly AppSettings _appSettings;

        public MapController(
            ILogger<MapController> logger,
            IMapService areaService,
            AppSettings appSettings)
        {
            _logger = logger;
            _mapService = areaService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _mapService.ToPageAsync(param);
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
            var data = await _mapService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] MapDto mapDto)
        {
            await _mapService.AddOrUpdateAsync(keyValue, mapDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _mapService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _mapService.Set().Where(e => e.IsEnable).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetExtends([FromQuery] string keyValue)
        {
            var map = Guard.NotNull(await _mapService.FirstOrDefaultAsync(e => e.Id == keyValue));
            var extends = _mapService.GetExtends(map);
            var data = extends.Select(e =>
            {
                var extend = new FASS.Data.Models.Map.Extend
                {
                    Key = e.Key,
                    Value = e.Value
                };
                return extend;
            }).ToList();
            return Ok(data);
        }

        [HttpPut]
        public IActionResult SaveSimpleProject([FromQuery] string map)
        {
            if (string.IsNullOrWhiteSpace(map))
                return BadRequest($"保存simple地图失败，参数map:[{map}]不能为空!");
            if (!map.Contains(".json"))
                return BadRequest($"保存simple地图失败，参数map:[{map}]无文件后缀(.json)!");
            try
            {
                var resp = WebAPIHelper.Instance.Get<SimpleResponse>(
                    $"{_appSettings.Mdcs.SimpleUrl}/saveProject/{map}"
                );
                if (resp.Success == true)
                {
                    return Ok(resp.Result);
                }
                else
                {
                    return BadRequest(resp.Result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"保存simple地图失败：{ex.Message}");
            }
        }
    }
}