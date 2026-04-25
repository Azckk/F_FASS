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
    [Tags("流程管理-任务实例-子任务-动作-参数")]
    public class TaskInstanceParameterController : BaseController
    {
        private readonly ILogger<TaskInstanceParameterController> _logger;
        private readonly ITaskInstanceParameterService _taskInstanceParameterService;

        public TaskInstanceParameterController(
            ILogger<TaskInstanceParameterController> logger,
            ITaskInstanceParameterService taskInstanceParameterService)
        {
            _logger = logger;
            _taskInstanceParameterService = taskInstanceParameterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskInstanceActionId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskInstanceParameterService.ToPageAsync(taskInstanceActionId, param);
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
            var data = await _taskInstanceParameterService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskInstanceParameterDto taskInstanceParameterDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                if (string.IsNullOrWhiteSpace(taskInstanceParameterDto.Key))
                {
                    return BadRequest($"键 [ {taskInstanceParameterDto.Key} ] 空值");
                }
                if (await _taskInstanceParameterService.AnyAsync(e => e.ActionId == taskInstanceParameterDto.ActionId && e.Key.ToUpper() == taskInstanceParameterDto.Key.ToUpper()))
                {
                    return BadRequest($"键 [ {taskInstanceParameterDto.Key} ] 已存在");
                }
            }
            await _taskInstanceParameterService.AddOrUpdateAsync(keyValue, taskInstanceParameterDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskInstanceParameterService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}