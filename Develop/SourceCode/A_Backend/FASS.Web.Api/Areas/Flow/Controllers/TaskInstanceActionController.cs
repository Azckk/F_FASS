using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Flow;
using FASS.Data.Dtos.Flow;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Flow.Controllers
{
    [Route("api/v1/Flow/[controller]/[action]")]
    [Tags("流程管理-任务实例-子任务-动作")]
    public class TaskInstanceActionController : BaseController
    {
        private readonly ILogger<TaskInstanceActionController> _logger;
        private readonly ITaskInstanceActionService _taskInstanceActionService;

        public TaskInstanceActionController(
            ILogger<TaskInstanceActionController> logger,
            ITaskInstanceActionService taskInstanceActionService)
        {
            _logger = logger;
            _taskInstanceActionService = taskInstanceActionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskInstanceProcessId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskInstanceActionService.ToPageAsync(taskInstanceProcessId, param);
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
            var data = await _taskInstanceActionService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskInstanceActionDto taskInstanceAction)
        {
            taskInstanceAction.State = TaskInstanceActionConst.State.Distributed;
            await _taskInstanceActionService.AddOrUpdateAsync(keyValue, taskInstanceAction);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskInstanceActionService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}