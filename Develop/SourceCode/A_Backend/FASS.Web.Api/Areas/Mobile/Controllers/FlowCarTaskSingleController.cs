using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Flow;
using FASS.Data.Dtos.Flow;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-单点任务")]
    public class FlowCarTaskSingleController : BaseController
    {
        private readonly ILogger<FlowCarTaskSingleController> _logger;
        private readonly ICarService _carService;
        private readonly INodeService _nodeService;
        private readonly ITaskInstanceService _taskInstanceService;

        public FlowCarTaskSingleController(
            ILogger<FlowCarTaskSingleController> logger,
            ICarService carService,
            INodeService nodeService,
            ITaskInstanceService taskInstanceService)
        {
            _logger = logger;
            _carService = carService;
            _nodeService = nodeService;
            _taskInstanceService = taskInstanceService;
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
        public async Task<IActionResult> Add([FromQuery] string carId, [FromQuery] string targetNodeId)
        {
            var targetNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == targetNodeId);
            if (targetNode == null)
            {
                return BadRequest($"获取目标站点失败 [{targetNodeId}]");
            }
            var car = string.IsNullOrEmpty(carId) ? default : await _carService.FirstOrDefaultAsync(e => e.IsEnable && e.Id == carId);
            var taskInstanceAdd = new TaskInstanceDto()
            {
                CarId = car?.Id,
                Code = $"{car?.Code}=>{targetNode.Code}",
                Name = $"车辆 [{car?.Code}] 去目标站点 [{targetNode.Code}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released,
                Nodes = [targetNode.Id]
            };
            _taskInstanceService.AddDto(taskInstanceAdd, "Single");
            var result = await _taskInstanceService.AnyAsync(e => e.Id == taskInstanceAdd.Id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}