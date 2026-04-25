using Common.NETCore.Models;
using FASS.Service.Services.Setting.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Areas.Setting.Controllers
{
    [Route("api/v1/Setting/[controller]/[action]")]
    [Tags("配置管理-充电策略配置")]
    public class ConfigChargeController : BaseController
    {
        private readonly ILogger<ConfigChargeController> _logger;
        private readonly IConfigChargeService _configChargeService;
        private readonly AppSettings _appSettings;
        private readonly IConfigServiceService _configServiceService;

        public ConfigChargeController(
            ILogger<ConfigChargeController> logger,
            IConfigChargeService configChargeService,
            AppSettings appSettings,
            IConfigServiceService configServiceService
        )
        {
            _logger = logger;
            _configServiceService = configServiceService;
            _configChargeService = configChargeService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _configChargeService.GetDtoAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> SetData(
            [FromBody] Service.Models.Set.ConfigChargeSet configDataDto
        )
        {
            try
            {
                var result = await _configChargeService.SetDtoAsync(configDataDto);
                if (result == 0)
                {
                    var resp = WebAPIHelper.Instance.Post<
                        ResponseResult,
                        Service.Models.Set.ConfigChargeSet
                    >($"{_appSettings.Mdcs.SimpleUrl}/car/setChargingSettings", configDataDto);
                    if (resp.Code == 200)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest($"配置充电参数失败 =>{resp.Message}");
                    }
                }
                else
                {
                    return BadRequest($"没有需要配置的参数");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"配置充电策略报--{e.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActionInit([FromBody] Service.Models.Set.ConfigChargeSet configDataDto)
        {
            try
            {
                var resp = WebAPIHelper.Instance.Post<
                    ResponseResult,
                    Service.Models.Set.ConfigChargeSet
                >($"{_appSettings.Mdcs.SimpleUrl}/car/setChargingSettings", configDataDto);
                if (resp.Code == 200)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest($"配置充电参数失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"配置充电参数失败 =>{ex}");
            }
        }
    }
}
