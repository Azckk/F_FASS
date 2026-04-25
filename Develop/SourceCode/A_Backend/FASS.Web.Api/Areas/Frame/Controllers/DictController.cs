using Common.AspNetCore.Extensions;
using Common.Frame.Dtos.Frame;
using Common.Frame.Services.Frame.Interfaces;
using Common.NETCore;
using Common.NETCore.Extensions;
using Common.Service.Pager.Models;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Frame.Controllers
{
    [Route("api/v1/Frame/[controller]/[action]")]
    [Tags("系统配置-字典管理")]
    public class DictController : BaseController
    {
        private readonly ILogger<DictController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IDictService _dictService;
        private readonly IDictItemService _dictItemService;

        public DictController(
            ILogger<DictController> logger,
            IDistributedCache distributedCache,
            IDictService dicKeyService,
            IDictItemService dictItemService)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _dictService = dicKeyService;
            _dictItemService = dictItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _dictService.ToPageAsync(param);
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
            var data = await _dictService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] DictDto dictDto)
        {
            await _dictService.AddOrUpdateAsync(keyValue, dictDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _dictService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetSync()
        {
            var data = await _dictItemService.ToListAsync(e => e.IsEnable);
            await _distributedCache.SetAsync(Utility.CacheKey.Setting.DictItem, data);
            return Ok();
        }
    }
}