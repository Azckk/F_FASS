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
    [Tags("流程管理-任务模板-子任务-动作-参数")]
    public class TaskTemplateParameterController : BaseController
    {
        private readonly ILogger<TaskTemplateParameterController> _logger;
        private readonly ITaskTemplateParameterService _taskTemplateParameterService;

        public TaskTemplateParameterController(
            ILogger<TaskTemplateParameterController> logger,
            ITaskTemplateParameterService taskTemplateParameterService)
        {
            _logger = logger;
            _taskTemplateParameterService = taskTemplateParameterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskTemplateActionId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskTemplateParameterService.ToPageAsync(taskTemplateActionId, param);
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
            var data = await _taskTemplateParameterService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskTemplateParameterDto taskTemplateParameterDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                if (string.IsNullOrWhiteSpace(taskTemplateParameterDto.Key))
                {
                    return BadRequest($"键 [ {taskTemplateParameterDto.Key} ] 空值");
                }
                if (await _taskTemplateParameterService.AnyAsync(e => e.ActionId == taskTemplateParameterDto.ActionId && e.Key.ToUpper() == taskTemplateParameterDto.Key.ToUpper()))
                {
                    return BadRequest($"键 [ {taskTemplateParameterDto.Key} ] 已存在");
                }
            }
            await _taskTemplateParameterService.AddOrUpdateAsync(keyValue, taskTemplateParameterDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskTemplateParameterService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}