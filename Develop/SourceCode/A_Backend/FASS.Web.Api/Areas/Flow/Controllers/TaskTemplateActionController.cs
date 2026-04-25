using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Flow;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Flow.Controllers
{
    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-任务模板-子任务-动作")]
    public class TaskTemplateActionController : BaseController
    {
        private readonly ILogger<TaskTemplateActionController> _logger;
        private readonly ITaskTemplateActionService _taskTemplateActionService;

        public TaskTemplateActionController(
            ILogger<TaskTemplateActionController> logger,
            ITaskTemplateActionService taskTemplateActionService)
        {
            _logger = logger;
            _taskTemplateActionService = taskTemplateActionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskTemplateProcessId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskTemplateActionService.ToPageAsync(taskTemplateProcessId, param);
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
            var data = await _taskTemplateActionService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskTemplateActionDto taskTemplateAction)
        {
            await _taskTemplateActionService.AddOrUpdateAsync(keyValue, taskTemplateAction);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskTemplateActionService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}