using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.DataExtend.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-交管配置")]
    public class TrafficRulesController : BaseController
    {
        private readonly ILogger<TrafficRulesController> _logger;
        private readonly ITrafficRulesService _trafficRulesService;
        private readonly AppSettings _appSettings;

        public TrafficRulesController(
            ILogger<TrafficRulesController> logger,
            ITrafficRulesService trafficRulesService,
            AppSettings appSettings
        )
        {
            _logger = logger;
            _trafficRulesService = trafficRulesService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _trafficRulesService.ToPageAsync(param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _trafficRulesService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TrafficRulesDto trafficRulesDto)
        {
            try
            {
                var result = await _trafficRulesService.AddOrUpdateAsync(keyValue, trafficRulesDto);
                if (result != 0)
                {
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, TrafficRulesDto>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/setTrafficControlSetting",
                        trafficRulesDto
                    );
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
                    return BadRequest($"没有需要配置或者修改的交管参数");
                }
            }
            catch (Exception e)
            {

                return BadRequest($"配置交管参数失败 =>{e.Message}");
            }
          
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            try
            {
                if (keyValues.Count() < 0)
                {
                    return BadRequest($"没有需要删除的交管参数");
                }
                var nameList = await _trafficRulesService
               .Set()
               .Where(e => keyValues.Contains(e.Id))
               .Select(e => e.Name)
               .ToListAsync();
                var result = await _trafficRulesService.DeleteAsync(keyValues);          
                if (result != 0)
                {
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, List<string>>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/delControlSetting",
                        nameList
                    );
                    if (resp.Code == 200)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest($"删除交管参数失败 =>{resp.Message}");
                    }
                }
                else
                {
                    return BadRequest($"没有需要删除的交管参数");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"删除的交管参数异常：{e.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _trafficRulesService
                .Set()
                .Where(e => e.IsEnable)
                .OrderByDescending(e => e.CreateAt)
                .ToListAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _trafficRulesService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _trafficRulesService.DisableAsync(keyValues);
            return Ok();
        }
    }
}
