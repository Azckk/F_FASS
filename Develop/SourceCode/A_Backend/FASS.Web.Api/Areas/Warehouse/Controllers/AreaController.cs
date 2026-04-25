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
    [Tags("仓储管理-区域管理")]
    public class AreaController : BaseController
    {
        private readonly ILogger<AreaController> _logger;
        private readonly IAreaService _areaService;

        public AreaController(
            ILogger<AreaController> logger,
            IAreaService areaService)
        {
            _logger = logger;
            _areaService = areaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _areaService.ToPageAsync(param);
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
            var data = await _areaService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] AreaDto areaDto)
        {
            await _areaService.AddOrUpdateAsync(keyValue, areaDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            try
            {
                await _areaService.DeleteAsync(keyValues);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("和库位存在绑定关系，请先删除接触绑定关系");
            }
         
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _areaService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetListByTypeToSelect([FromQuery] string type)
        {
            var data = await _areaService.Set().Where(e => e.IsEnable && e.Type.ToUpper() == type.ToUpper()).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }
    }
}
