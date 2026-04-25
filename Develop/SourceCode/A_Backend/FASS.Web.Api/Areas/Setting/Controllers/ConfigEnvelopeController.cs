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
    [Tags("配置管理-包络配置")]
    public class ConfigEnvelopeController : BaseController
    {
        private readonly ILogger<ConfigEnvelopeController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfigEnvelopeService _configEnvelopeService;
        private readonly AppSettings _appSettings;

        public ConfigEnvelopeController(
            ILogger<ConfigEnvelopeController> logger,
            IDistributedCache distributedCache,
            IConfigEnvelopeService configEnvelopeService,
            AppSettings appSettings
        )
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configEnvelopeService = configEnvelopeService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _configEnvelopeService.GetDtoAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> SetData([FromBody] ConfigEnvelopeSet configDataDto)
        {
            try
            {
                var result = await _configEnvelopeService.SetDtoAsync(configDataDto);
                if (result == 0)
                {
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, ConfigEnvelopeSet>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/setEnvelopeSetting",
                        configDataDto
                    );
                    if (resp.Code == 200)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest($"配置包络参数失败 =>{resp.Message}");
                    }
                }
                else
                {
                    return BadRequest($"没有需要包络配置的参数");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"包络配置修改参数报错--{e.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActionInit([FromBody] ConfigEnvelopeSet configDataDto)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Post<ResponseResult, ConfigEnvelopeSet>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/setEnvelopeSetting",
                    configDataDto
                );
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"配置包络参数失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"配置包络参数失败 =>{ex}");
            }
        }
    }
}
