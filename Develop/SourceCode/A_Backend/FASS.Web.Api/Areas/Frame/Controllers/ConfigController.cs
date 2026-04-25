using Common.Frame.Dtos.Frame;
using Common.Frame.Services.Frame.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Common.AspNetCore.Extensions;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Frame.Controllers
{
    [Route("api/v1/Frame/[controller]/[action]")]
    [Tags("系统配置-配置管理")]
    public class ConfigController : BaseController
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfigService _configService;

        public ConfigController(
            ILogger<ConfigController> logger,
            IDistributedCache distributedCache,
            IConfigService configService)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configService = configService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _configService.ToPageAsync(param);
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
            var data = await _configService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] ConfigDto configDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                if (string.IsNullOrWhiteSpace(configDto.Key))
                {
                    return BadRequest($"键 [{configDto.Key}] 空值");
                }
                if (await _configService.AnyKeyAsync(configDto.Key))
                {
                    return BadRequest($"键 [{configDto.Key}] 已存在");
                }
            }
            await _configService.AddOrUpdateAsync(keyValue, configDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _configService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetSync()
        {
            var data = await _configService.ToListAsync(e => e.IsEnable);
            await _distributedCache.SetAsync(Utility.CacheKey.Setting.Config, data);
            return Ok();
        }
    }
}