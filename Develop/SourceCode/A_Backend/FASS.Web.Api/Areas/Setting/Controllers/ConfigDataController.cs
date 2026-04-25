using Common.AspNetCore.Extensions;
using FASS.Data.Dtos.Setting;
using FASS.Data.Services.Setting.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Setting.Controllers
{
    [Route("api/v1/Setting/[controller]/[action]")]
    [Tags("配置管理-数据配置")]
    public class ConfigDataController : BaseController
    {
        private readonly ILogger<ConfigDataController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfigDataService _configDataService;

        public ConfigDataController(
            ILogger<ConfigDataController> logger,
            IDistributedCache distributedCache,
            IConfigDataService configDataService)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configDataService = configDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _configDataService.GetDtoAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> SetData([FromBody] ConfigDataDto configDataDto)
        {
            await _configDataService.SetDtoAsync(configDataDto);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetSync()
        {
            var data = await _configDataService.GetDtoAsync();
            await _distributedCache.SetAsync(Utility.CacheKey.Setting.ConfigData, data);
            return Ok();
        }
    }
}