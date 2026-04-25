using FASS.Boot.Services;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Instant;
using FASS.Data.Models.Data;
using FASS.Data.Models.Flow;
using FASS.Data.Models.Instant;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Data.Services.Instant.Interfaces;
using FASS.Scheduler.Attributes;
using FASS.Scheduler.Controllers.Base;
using FASS.Scheduler.Controllers.Extensions;
using FASS.Service.Consts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Scheduler.Controllers
{
    [AllowAnonymous]
    [TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
    [TypeFilter(typeof(ActionLogIgonreAttribute))]
    [Tags("调度接口")]
    public class CarController : BaseController
    {
        private readonly ILogger<CarController> _logger;
        private readonly IBootService _bootService;
        private readonly ICarService _carService;
        private readonly INodeService _nodeService;
        private readonly ICarActionService _carActionService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly ITaskTemplateService _taskTemplateService;
        private readonly ICarInstantActionService _carInstantActionService;

        public CarController(
            ILogger<CarController> logger,
            IBootService bootService,
            ICarService carService,
            INodeService nodeService,
            ICarActionService carActionService,
            ITaskInstanceService taskInstanceService,
            ITaskTemplateService taskTemplateService,
            ICarInstantActionService carInstantActionService)
        {
            _logger = logger;
            _bootService = bootService;
            _carService = carService;
            _nodeService = nodeService;
            _carActionService = carActionService;
            _taskInstanceService = taskInstanceService;
            _taskTemplateService = taskTemplateService;
            _carInstantActionService = carInstantActionService;
        }

        [Tags("状态")]
        [HttpPost]
        public IActionResult State(Models.Request.State request)
        {
            if (string.IsNullOrEmpty(request.CarCode))
            {
                var currCars = _bootService.Cars.ToList();
                var responses = currCars.Select(e => e.ToState()).ToList();
                return Ok(responses);
            }
            var car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
            if (car == null)
            {
                return BadRequest($"获取车辆失败 [{request.CarCode}]");
            }
            var response = car.ToState();
            return Ok(response);
        }

        [Tags("任务（单点）")]
        [HttpPost]
        public IActionResult TaskSingle(Models.Request.TaskSingle request)
        {
            var targetNode = _bootService.Nodes.FirstOrDefault(e => e.Code == request.TargetNodeCode);
            if (targetNode == null)
            {
                return BadRequest($"获取目标站点失败 [{request.TargetNodeCode}]");
            }
            Car? car = null;
            if (!string.IsNullOrEmpty(request.CarCode))
            {
                car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{request.CarCode}]");
                }
            }
            var taskInstanceAdd = new TaskInstance()
            {
                CarId = car?.Id,
                Code = $"{request.CarCode}=>{targetNode.Code}",
                Name = $"车辆 [{request.CarCode}] 去目标站点 [{targetNode.Code}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released,
                Nodes = [targetNode.Id]
            };
            _bootService.AddTaskInstance(taskInstanceAdd, "Single");
            var taskInstance = taskInstanceAdd.ToTaskInstance();
            return Ok(taskInstance);
        }

        [Tags("任务（双点）")]
        [HttpPost]
        public IActionResult TaskDouble(Models.Request.TaskDouble request)
        {
            var startNode = _bootService.Nodes.FirstOrDefault(e => e.Code == request.StartNodeCode);
            if (startNode == null)
            {
                return BadRequest($"获取起点站点失败 [{request.StartNodeCode}]");
            }
            var endNode = _bootService.Nodes.FirstOrDefault(e => e.Code == request.EndNodeCode);
            if (endNode == null)
            {
                return BadRequest($"获取终点站点失败 [{request.EndNodeCode}]");
            }
            Car? car = null;
            if (!string.IsNullOrEmpty(request.CarCode))
            {
                car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{request.CarCode}]");
                }
            }
            var taskInstanceAdd = new TaskInstance()
            {
                CarId = car?.Id,
                Code = $"{request.CarCode}=>[{startNode.Code}=>{endNode.Code}]",
                Name = $"车辆 [{request.CarCode}] 从起点站点 [{startNode.Code}] 到终点站点 [{endNode.Code}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released,
                Nodes = [startNode.Id, endNode.Id]
            };
            _bootService.AddTaskInstance(taskInstanceAdd, "Double");
            var taskInstance = taskInstanceAdd.ToTaskInstance();
            return Ok(taskInstance);
        }

        [Tags("任务（模板）")]
        [HttpPost]
        public IActionResult TaskTemplate(Models.Request.TaskTemplate request)
        {
            var taskTemplate = _taskTemplateService.FirstOrDefault(e => e.Code == request.TaskTemplateCode);
            if (taskTemplate == null)
            {
                return BadRequest($"获取任务模板失败 [{request.TaskTemplateCode}]");
            }
            Car? car = null;
            if (!string.IsNullOrEmpty(request.CarCode))
            {
                car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{request.CarCode}]");
                }
            }
            var taskInstanceAdd = new TaskInstance()
            {
                CarId = car?.Id,
                Code = $"{request.CarCode}=>{taskTemplate.Code}",
                Name = $"车辆 [{request.CarCode}] 执行任务模板 [{taskTemplate.Code}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released,
                TaskTemplateId = taskTemplate.Id
            };
            _bootService.AddTaskInstance(taskInstanceAdd);
            var taskInstance = taskInstanceAdd.ToTaskInstance();
            return Ok(taskInstance);
        }

        [Tags("任务（模板参数）")]
        [HttpPost]
        public IActionResult TaskTemplateParameter(Models.Request.TaskTemplateParam request)
        {
            var taskTemplate = _taskTemplateService.FirstOrDefault(e => e.Code == request.TaskTemplateCode);
            if (taskTemplate == null)
            {
                return BadRequest($"获取任务模板失败 [{request.TaskTemplateCode}]");
            }
            Car? car = null;
            if (!string.IsNullOrEmpty(request.CarCode))
            {
                car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{request.CarCode}]");
                }
            }
            var taskInstanceAdd = new TaskInstance()
            {
                CarId = car?.Id,
                Code = $"{request.CarCode}=>{taskTemplate.Code}",
                Name = $"车辆 [{request.CarCode}] 执行任务模板 [{taskTemplate.Code}] 参数 站点 [{request.TaskTemplateNodeCodes}] 路线 [{request.TaskTemplateEdgeCodes}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released,
                TaskTemplateId = taskTemplate.Id
            };
            if (!string.IsNullOrEmpty(request.TaskTemplateNodeCodes))
            {
                taskInstanceAdd.Nodes = request.TaskTemplateNodeCodes.Split(',');
            }
            if (!string.IsNullOrEmpty(request.TaskTemplateEdgeCodes))
            {
                taskInstanceAdd.Edges = request.TaskTemplateEdgeCodes.Split(',');
            }
            _bootService.AddTaskInstance(taskInstanceAdd);
            var taskInstance = taskInstanceAdd.ToTaskInstance();
            return Ok(taskInstance);
        }

        [Tags("动作")]
        [HttpPost]
        public IActionResult Action(Models.Request.Action request)
        {
            var car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
            if (car == null)
            {
                return BadRequest($"获取车辆失败 [{request.CarCode}]");
            }
            var carActionType = _carActionService.FirstOrDefault(e => e.CarTypeId == car.CarTypeId && e.Code == request.ActionType);
            if (carActionType == null)
            {
                return BadRequest($"获取车辆动作类型失败 [{request.ActionType}]");
            }
            var carInstantActionAdd = new CarInstantAction()
            {
                CarId = car.Id,
                ActionType = request.ActionType,
                BlockingType = CarInstantActionConst.BlockingType.NONE,
                State = TaskInstanceConst.State.Released,
                Remark = $"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 动作 [{request.ActionType}]"
            };
            if (request.Parameters != null && request.Parameters.Any())
            {
                carInstantActionAdd.CarInstantParameters = request.Parameters.Select(e => new CarInstantParameter() { ActionId = carInstantActionAdd.Id, Key = e.Key, Value = e.Value }).ToList();
                carInstantActionAdd.Remark += $" 参数 [{string.Join(";", request.Parameters.Select(e => $"Key:{e.Key},Value:{e.Value}"))}]";
            }
            _bootService.AddCarInstantAction(carInstantActionAdd);
            var carInstantAction = carInstantActionAdd.ToCarInstantAction();
            return Ok(carInstantAction);
        }

        [Tags("动作（启动）")]
        [HttpPost]
        public IActionResult ActionStart(Models.Request.Action request) => Action(new Models.Request.Action() { CarCode = request.CarCode, ActionType = CarActionConst.Type.Start });

        [Tags("动作（停止）")]
        [HttpPost]
        public IActionResult ActionStop(Models.Request.Action request) => Action(new Models.Request.Action() { CarCode = request.CarCode, ActionType = CarActionConst.Type.Stop });

        [Tags("动作（参数）")]
        [HttpPost]
        public IActionResult ActionParameter(Models.Request.Action request) => Action(request);

        [Tags("站点动作")]
        [HttpPost]
        public IActionResult NodeAction(Models.Request.NodeAction request)
        {
            var node = _bootService.Nodes.FirstOrDefault(e => e.Code == request.NodeCode);
            if (node == null)
            {
                return BadRequest($"获取站点失败 [{request.NodeCode}]");
            }
            var car = _bootService.Cars.FirstOrDefault(e => e.CurrNode?.Code == node.Code);
            if (car == null)
            {
                return BadRequest($"获取站点车辆失败 [{request.NodeCode}]");
            }
            var carActionType = _carActionService.FirstOrDefault(e => e.CarTypeId == car.CarTypeId && e.Code == request.ActionType);
            if (carActionType == null)
            {
                return BadRequest($"获取车辆动作类型失败 [{request.ActionType}]");
            }
            var carInstantActionAdd = new CarInstantAction()
            {
                CarId = car.Id,
                ActionType = request.ActionType,
                BlockingType = CarInstantActionConst.BlockingType.NONE,
                State = TaskInstanceConst.State.Released,
                Remark = $"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 动作 [{request.ActionType}]"
            };
            if (request.Parameters != null && request.Parameters.Any())
            {
                carInstantActionAdd.CarInstantParameters = request.Parameters.Select(e => new CarInstantParameter() { ActionId = carInstantActionAdd.Id, Key = e.Key, Value = e.Value }).ToList();
                carInstantActionAdd.Remark += $" 参数 [{string.Join(";", request.Parameters.Select(e => $"Key:{e.Key},Value:{e.Value}"))}]";
            }
            _bootService.AddCarInstantAction(carInstantActionAdd);
            var carInstantAction = carInstantActionAdd.ToCarInstantAction();
            return Ok(carInstantAction);
        }

        [Tags("站点动作（启动）")]
        [HttpPost]
        public IActionResult NodeActionStart(Models.Request.NodeAction request) => NodeAction(new Models.Request.NodeAction() { NodeCode = request.NodeCode, ActionType = CarActionConst.Type.Start });

        [Tags("站点动作（停止）")]
        [HttpPost]
        public IActionResult NodeActionStop(Models.Request.NodeAction request) => NodeAction(new Models.Request.NodeAction() { NodeCode = request.NodeCode, ActionType = CarActionConst.Type.Stop });

        [Tags("站点动作（参数）")]
        [HttpPost]
        public IActionResult NodeActionParameter(Models.Request.NodeAction request) => NodeAction(request);
    }
}