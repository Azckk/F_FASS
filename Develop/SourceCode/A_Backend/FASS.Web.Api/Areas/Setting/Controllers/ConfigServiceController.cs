using Common.AspNetCore.Extensions;
using FASS.Service.Dtos.Setting;
using FASS.Service.Services.Setting.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Setting.Controllers
{
    [Route("api/v1/Setting/[controller]/[action]")]
    [Tags("配置管理-服务配置")]
    public class ConfigServiceController : BaseController
    {
        private readonly ILogger<ConfigServiceController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfigServiceService _configServiceService;

        public ConfigServiceController(
            ILogger<ConfigServiceController> logger,
            IDistributedCache distributedCache,
            IConfigServiceService configServiceService)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configServiceService = configServiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _configServiceService.GetDtoAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> SetData([FromBody] ConfigServiceDto configServiceDto)
        {
            await _configServiceService.SetDtoAsync(configServiceDto);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetSync()
        {
            var data = await _configServiceService.GetDtoAsync();
            await _distributedCache.SetAsync(Utility.CacheKey.Setting.ConfigService, data);
            return Ok();
        }
    }
}