using Common.Frame.Dtos.Frame;
using Common.Frame.Services.Frame.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Frame.Controllers
{
    [Route("api/v1/Frame/[controller]/[action]")]
    [Tags("系统配置-字典项管理")]
    public class DictItemController : BaseController
    {
        private readonly ILogger<DictItemController> _logger;
        private readonly IDictItemService _dictItemService;

        public DictItemController(
            ILogger<DictItemController> logger,
            IDictItemService dicValueService)
        {
            _logger = logger;
            _dictItemService = dicValueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string dictId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _dictItemService.ToPageAsync(dictId, param);
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
            var data = await _dictItemService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] DictItemDto dictItemDto)
        {
            await _dictItemService.AddOrUpdateAsync(keyValue, dictItemDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _dictItemService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect([FromQuery] string dictCode)
        {
            var data = await _dictItemService.Set().Where(e => e.IsEnable && e.Dict.Code == dictCode).OrderBy(e => e.SortNumber).ToListAsync();
            return Ok(data);
        }
    }
}