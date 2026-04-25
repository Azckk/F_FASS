using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Flow;
using FASS.Data.Dtos.Flow;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-模板任务")]
    public class FlowCarTaskTemplateController : BaseController
    {
        private readonly ILogger<FlowCarTaskTemplateController> _logger;
        private readonly ICarService _carService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly ITaskTemplateService _taskTemplateService;
        private readonly ITaskTemplateProcessService _taskTemplateProcessService;

        public FlowCarTaskTemplateController(
            ILogger<FlowCarTaskTemplateController> logger,
            ICarService carService,
            ITaskInstanceService taskInstanceService,
            ITaskTemplateService taskTemplateService,
            ITaskTemplateProcessService taskTemplateProcessService)
        {
            _logger = logger;
            _carService = carService;
            _taskInstanceService = taskInstanceService;
            _taskTemplateService = taskTemplateService;
            _taskTemplateProcessService = taskTemplateProcessService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _taskInstanceService.ToPageAsync(param);
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
            var data = await _taskInstanceService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromQuery] string carId, [FromQuery] string taskTemplateId)
        {
            var taskTemplate = await _taskTemplateService.FirstOrDefaultAsync(e => e.Id == taskTemplateId);
            if (taskTemplate == null)
            {
                return BadRequest($"获取任务模板失败 [{taskTemplateId}]");
            }
            var taskTemplateProcess = await _taskTemplateProcessService.ToListAsync(e => e.TaskTemplateId == taskTemplate.Id);
            if (taskTemplateProcess.Any(e => string.IsNullOrEmpty(e.NodeId) && string.IsNullOrEmpty(e.EdgeId)))
            {
                return BadRequest($"任务创建失败,模板子任务未配置站点或路线 [{taskTemplateId}]");
            }
            var car = string.IsNullOrEmpty(carId) ? default : await _carService.FirstOrDefaultAsync(e => e.IsEnable && e.Id == carId);
            var taskInstanceAdd = new TaskInstanceDto()
            {
                TaskTemplateId = taskTemplate.Id,
                CarId = car?.Id,
                Code = $"{car?.Code}=>{taskTemplate.Code}",
                Name = $"车辆 [{car?.Code}] 执行任务模板 [{taskTemplate.Code}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released
            };
            _taskInstanceService.AddDto(taskInstanceAdd);
            var result = await _taskInstanceService.AnyAsync(e => e.Id == taskInstanceAdd.Id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}