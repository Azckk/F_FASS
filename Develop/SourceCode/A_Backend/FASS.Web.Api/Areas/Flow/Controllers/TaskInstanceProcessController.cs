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
    [Tags("流程管理-任务实例-子任务")]
    public class TaskInstanceProcessController : BaseController
    {
        private readonly ILogger<TaskInstanceProcessController> _logger;
        private readonly ITaskInstanceProcessService _taskInstanceProcessService;

        public TaskInstanceProcessController(
            ILogger<TaskInstanceProcessController> logger,
            ITaskInstanceProcessService taskInstanceProcessService)
        {
            _logger = logger;
            _taskInstanceProcessService = taskInstanceProcessService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string taskInstanceId, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskInstanceProcessService.ToPageAsync(taskInstanceId, param);
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
            var data = await _taskInstanceProcessService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] TaskInstanceProcessDto taskInstanceProcess)
        {
            taskInstanceProcess.State = TaskInstanceProcessConst.State.Distributed;
            await _taskInstanceProcessService.AddOrUpdateAsync(keyValue, taskInstanceProcess);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _taskInstanceProcessService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}