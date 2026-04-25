using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.FlowExtend.Controllers
{

    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-任务模板(Mdcs)-规则")]
    public class TaskTemplateRuleController : BaseController
    {
        private readonly ILogger<TaskTemplateRuleController> _logger;
        private readonly ITaskTemplateRuleService _taskTemplateRuleService;

        public TaskTemplateRuleController(
            ILogger<TaskTemplateRuleController> logger,
            ITaskTemplateRuleService taskTemplateRuleService)
        {
            _logger = logger;
            _taskTemplateRuleService = taskTemplateRuleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskTemplateId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskTemplateRuleService.ToPageAsync(taskTemplateId, param);
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
            var data = await _taskTemplateRuleService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskTemplateRuleDto taskTemplateRule)
        {
            await _taskTemplateRuleService.AddOrUpdateAsync(keyValue, taskTemplateRule);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskTemplateRuleService.DeleteAsync(keyValues);
            return Ok();
        }
    }

}
