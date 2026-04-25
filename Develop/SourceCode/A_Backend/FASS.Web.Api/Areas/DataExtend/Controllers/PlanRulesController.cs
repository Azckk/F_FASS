using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Service.Dtos.DataExtend;
using FASS.Service.Services.DataExtend;
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
    [Tags("数据管理-路径规划配置")]
    public class PlanRulesController : BaseController
    {
        private readonly ILogger<PlanRulesController> _logger;
        private readonly IPlanRulesService _planRulesService;
        private readonly AppSettings _appSettings;
        public PlanRulesController(
            ILogger<PlanRulesController> logger,
            IPlanRulesService planRulesService, AppSettings appSettings)
        {
            _logger = logger;
            _planRulesService = planRulesService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _planRulesService.ToPageAsync(param);
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
            var data = await _planRulesService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] PlanRulesDto planRulesDto)
        {
            try
            {
              var result=  await _planRulesService.AddOrUpdateAsync(keyValue, planRulesDto);
                if (result != 0)
                {
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, PlanRulesDto>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/setPlanRulesControlSetting",
                        planRulesDto
                    );
                    if (resp.Code == 200)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest($"配置路径规划参数失败 =>{resp.Message}");
                    }
                }
                else
                {
                    return BadRequest($"没有需要配置或者修改的路径规划参数");
                }
            }
            catch (Exception e)
            {

                return BadRequest($"配置路径规划参数失败 =>{e.Message}"); ;
            }
    
 
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
         /*   await _planRulesService.DeleteAsync(keyValues);
            return Ok();*/

            try
            {
                if (keyValues.Count() < 0)
                {
                    return BadRequest($"没有需要删除的交管参数");
                }
                var nameList = await _planRulesService
               .Set()
               .Where(e => keyValues.Contains(e.Id))
               .Select(e => new PlanRulesDto { Name=e.Name,NodeId=e.NodeId})
               .ToListAsync();
                var result = await _planRulesService.DeleteAsync(keyValues);
                if (result != 0)
                {
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, List<PlanRulesDto>>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/delPlanRulesControlSetting",
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
            var data = await _planRulesService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _planRulesService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _planRulesService.DisableAsync(keyValues);
            return Ok();
        }


    }
}
