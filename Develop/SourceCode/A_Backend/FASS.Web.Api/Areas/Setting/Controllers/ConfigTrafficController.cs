using Common.Utility.Extensions;
using Common.Utility.Models;
using FASS.Service.Models.Set;
using FASS.Service.Services.Setting.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Setting.Controllers
{
    [Route("api/v1/Setting/[controller]/[action]")]
    [Tags("配置管理-交管配置")]
    public class ConfigTrafficController : BaseController
    {
        private readonly ILogger<ConfigTrafficController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfigTrafficControlService _configServiceService;
        private readonly AppSettings _appSettings;

        public ConfigTrafficController(
            ILogger<ConfigTrafficController> logger,
            IDistributedCache distributedCache,
            IConfigTrafficControlService configServiceService, AppSettings appSettings)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configServiceService = configServiceService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _configServiceService.GetDtoAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> SetData([FromBody] ConfigTrafficControlSet configServiceDto)
        {
            var result=await _configServiceService.SetDtoAsync(configServiceDto);
            if (result == 0)
            {
                configServiceDto.MapUrl = $"{_appSettings.Mdcs.MapUrl}";
                var resp = WebAPIHelper.Instance.Post<
                    ResponseResult,
                    ConfigTrafficControlSet
                >($"{_appSettings.Mdcs.SimpleUrl}/car/setTrafficControlSetting", configServiceDto);
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"配置交管参数失败 =>{resp.Message}");
                }
            }
            else
            {
                return BadRequest($"没有需要配置的交管参数");
            }
          
        }

    
    }
}