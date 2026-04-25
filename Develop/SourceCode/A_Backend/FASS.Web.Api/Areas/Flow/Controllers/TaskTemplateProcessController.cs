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
    [Tags("流程管理-任务模板-子任务")]
    public class TaskTemplateProcessController : BaseController
    {
        private readonly ILogger<TaskTemplateProcessController> _logger;
        private readonly ITaskTemplateProcessService _taskTemplateProcessService;

        public TaskTemplateProcessController(
            ILogger<TaskTemplateProcessController> logger,
            ITaskTemplateProcessService taskTemplateProcessService)
        {
            _logger = logger;
            _taskTemplateProcessService = taskTemplateProcessService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskTemplateId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskTemplateProcessService.ToPageAsync(taskTemplateId, param);
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
            var data = await _taskTemplateProcessService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskTemplateProcessDto taskTemplateProcess)
        {
            await _taskTemplateProcessService.AddOrUpdateAsync(keyValue, taskTemplateProcess);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskTemplateProcessService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}