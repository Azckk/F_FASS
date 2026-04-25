using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Services.DataExtend.Interfaces;
using FASS.Service.Services.Setting.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.DataExtend.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-包络配置")]
    public class EnvelopeController : BaseController
    {
        private readonly ILogger<EnvelopeController> _logger;
        private readonly IEnvelopeService _envelopeService;
        private readonly AppSettings _appSettings;
        private readonly IConfigServiceService _configServiceService;

        public EnvelopeController(
            ILogger<EnvelopeController> logger,
            IEnvelopeService envelopeService,
            AppSettings appSettings,
            IConfigServiceService configServiceService
        )
        {
            _logger = logger;
            _envelopeService = envelopeService;
            _appSettings = appSettings;
            _configServiceService = configServiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _envelopeService.ToPageAsync(param);
            var data = new { rows = page.Data, total = page.TotalCount };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _envelopeService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue,[FromBody] EnvelopeDto envelopeDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyValue))
                    envelopeDto.IsEnable = false; //新增默认不启用
                var result = await _envelopeService.AddOrUpdateAsync(keyValue, envelopeDto);
                if (result != 0 && envelopeDto.IsEnable == true)
                {
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, EnvelopeDto>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/setEnvelopeSetting",
                        envelopeDto
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
                    return Ok($"暂时不启用包络配置的参数");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"配置包络参数失败 =>{e.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            try
            {
                var nameList = await _envelopeService
                    .Set()
                    .Where(e => keyValues.Contains(e.Id))
                    .Select(e => e.Name)
                    .ToListAsync();
                var mapUrl = await _configServiceService
                    .Set()
                    .Where(e => e.Key == "mapUrl")
                    .Select(e => e.Value)
                    .FirstOrDefaultAsync();
                var result = await _envelopeService.DeleteAsync(keyValues);
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
                        return BadRequest($"删除包络参数失败 =>{resp.Message}");
                    }
                }
                else
                {
                    return BadRequest($"没有需要删除的包络参数");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"删除的包络参数报错:{e.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _envelopeService
                .Set()
                .Where(e => e.IsEnable)
                .OrderByDescending(e => e.CreateAt)
                .ToListAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _envelopeService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _envelopeService.DisableAsync(keyValues);
            return Ok();
        }
    }
}
