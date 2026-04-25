using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Flow;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-任务区域A")]
    public class WarehouseWorkAreaAController : BaseController
    {
        private readonly ILogger<WarehouseWorkAreaAController> _logger;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly IWorkService _workService;
        private readonly INodeService _nodeService;
        private readonly ITaskTemplateService _taskTemplateService;
        private readonly ITaskTemplateProcessService _taskTemplateProcessService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly ITaskInstanceProcessService _taskInstanceProcessService;
        private readonly ITaskInstanceActionService _taskInstanceActionService;

        public WarehouseWorkAreaAController(
            ILogger<WarehouseWorkAreaAController> logger,
            IStorageService storageService,
            IContainerService containerService,
            IMaterialService materialService,
            IWorkService workService,
            INodeService nodeService,
            ITaskTemplateService taskTemplateService,
            ITaskTemplateProcessService taskTemplateProcessService,
            ITaskInstanceService taskInstanceService,
            ITaskInstanceProcessService taskInstanceProcessService,
            ITaskInstanceActionService taskInstanceActionService)
        {
            _logger = logger;
            _storageService = storageService;
            _containerService = containerService;
            _materialService = materialService;
            _workService = workService;
            _nodeService = nodeService;
            _taskTemplateService = taskTemplateService;
            _taskTemplateProcessService = taskTemplateProcessService;
            _taskInstanceService = taskInstanceService;
            _taskInstanceProcessService = taskInstanceProcessService;
            _taskInstanceActionService = taskInstanceActionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _workService.ToPageAsync(param);
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
            var data = await _workService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        //public IActionResult Pick()
        //{
        //    return View();
        //}

        [HttpPut]
        public async Task<IActionResult> Pick([FromQuery] string containerId, [FromBody] string[] materialIds)
        {
            var container = await _containerService.FirstOrDefaultAsync(e => e.Id == containerId);
            if (container is null)
            {
                return BadRequest($"容器不存在，容器编号 [{containerId}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage is null)
            {
                return BadRequest($"容器储位不存在，容器编码 [{container.Code}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode is null)
            {
                return BadRequest($"容器储位站点不存在，站点编号 [{containerStorage.NodeId}]");
            }
            var containerMaterials = await _materialService.ToListAsync(e => materialIds.Contains(e.Id));
            if (containerMaterials is null || containerMaterials.Count == 0)
            {
                return BadRequest($"容器物料不存在，物料编号 [{string.Join(',', materialIds.Select(e => $"{e}"))}]");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 is not null && e2 is not null && e1 == e2).ToList();
            if (materialStorages is null || materialStorages.Count == 0)
            {
                return BadRequest($"物料储位不存在，物料编码 [{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes is null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"物料储位站点不存在，站点编号 [{string.Join(',', materialStorages.Select(e => $"{e.NodeId}"))}]");
            }

            await _containerService.MaterialAddAsync(containerId, containerMaterials.Select(e => e.Id));

            var workAdd = new WorkDto
            {
                ContainerId = container.Id,
                Code = $"{container.Code}=>[{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]",
                Name = $"容器 [{container.Code}] 绑定物料 [{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]",
                Type = WorkConst.Type.Normal,
                State = WorkConst.State.Created
            };
            _workService.Add(workAdd);

            //container.State = ContainerConst.State.FullMaterial;
            //container.IsLock = true;
            //_containerService.Update(container);

            //containerMaterials.ForEach(e => e.State = MaterialConst.State.Bind);
            //_materialService.Update(containerMaterials);

            return Ok();
        }

        //public IActionResult Drop()
        //{
        //    return View();
        //}

        [HttpPut]
        public async Task<IActionResult> Drop([FromQuery] string containerId, [FromQuery] string storageId)
        {
            var container = await _containerService.FirstOrDefaultAsync(e => e.Id == containerId);
            if (container is null)
            {
                return BadRequest($"容器不存在，容器编号 [{containerId}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage is null)
            {
                return BadRequest($"容器储位不存在，容器编码 [{container.Code}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode is null)
            {
                return BadRequest($"容器储位站点不存在，站点编号 [{containerStorage.NodeId}]");
            }
            var containerMaterials = _containerService.GetMaterials(container).ToList();
            if (containerMaterials is null || containerMaterials.Count == 0)
            {
                return BadRequest($"容器物料不存在，容器编码 [{container.Code}]");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 is not null && e2 is not null && e1 == e2).ToList();
            if (materialStorages is null || materialStorages.Count == 0)
            {
                return BadRequest($"物料储位不存在，物料编码 [{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes is null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"物料储位站点不存在，站点编号 [{string.Join(',', materialStorages.Select(e => $"{e.NodeId}"))}]");
            }
            var storage = await _storageService.FirstOrDefaultAsync(e => e.Id == storageId);
            if (storage is null)
            {
                return BadRequest($"储位不存在，储位编号 [{storageId}]");
            }
            var storageMaterials = _storageService.GetMaterials(storage).ToList();
            if (storageMaterials is null || storageMaterials.Count == 0)
            {
                return BadRequest($"储位物料不存在，储位编码 [{storage.Code}]");
            }

            await _containerService.MaterialDeleteAsync(containerId, storageMaterials.Select(e => e.Id));

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Remove([FromQuery] string keyValue)
        {
            var work = await _workService.FirstOrDefaultAsync(e => e.Id == keyValue);
            if (work is null)
            {
                return BadRequest($"任务不存在，任务编号 [{keyValue}]");
            }
            if (work.State != WorkConst.State.Created)
            {
                return BadRequest($"任务状态不正确，任务状态 [{WorkConst.State.Created}]");
            }
            var container = await _containerService.FirstOrDefaultAsync(e => e.Id == work.ContainerId);
            if (container is null)
            {
                return BadRequest($"容器不存在，容器编号 [{work.ContainerId}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage is null)
            {
                return BadRequest($"容器储位不存在，容器编码 [{container.Code}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode is null)
            {
                return BadRequest($"容器储位站点不存在，站点编号 [{containerStorage.NodeId}]");
            }
            var containerMaterials = _containerService.GetMaterials(container).ToList();
            if (containerMaterials is null || containerMaterials.Count == 0)
            {
                return BadRequest($"容器物料不存在，容器编码 [{container.Code}]");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 is not null && e2 is not null && e1 == e2).ToList();
            if (materialStorages is null || materialStorages.Count == 0)
            {
                return BadRequest($"物料储位不存在，物料编码 [{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes is null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"物料储位站点不存在，站点编号 [{string.Join(',', materialStorages.Select(e => $"{e.NodeId}"))}]");
            }

            await _containerService.MaterialDeleteAsync(container.Id, containerMaterials.Select(e => e.Id));

            await _workService.DeleteAsync(work);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Task([FromQuery] string keyValue)
        {
            var work = await _workService.FirstOrDefaultAsync(e => e.Id == keyValue);
            if (work is null)
            {
                return BadRequest($"任务不存在，任务编号 [{keyValue}]");
            }
            if (work.State != WorkConst.State.Created)
            {
                return BadRequest($"任务状态不正确，任务状态 [{WorkConst.State.Created}]");
            }
            var container = await _containerService.FirstOrDefaultAsync(e => e.Id == work.ContainerId);
            if (container is null)
            {
                return BadRequest($"容器不存在，容器编号 [{work.ContainerId}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage is null)
            {
                return BadRequest($"容器储位不存在，容器编码 [{container.Code}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode is null)
            {
                return BadRequest($"容器储位站点不存在，站点编号 [{containerStorage.NodeId}]");
            }
            var containerMaterials = _containerService.GetMaterials(container).ToList();
            if (containerMaterials is null || containerMaterials.Count == 0)
            {
                return BadRequest($"容器物料不存在，容器编码 [{container.Code}]");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 is not null && e2 is not null && e1 == e2).ToList();
            if (materialStorages is null || materialStorages.Count == 0)
            {
                return BadRequest($"物料储位不存在，物料编码 [{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes is null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"物料储位站点不存在，站点编号 [{string.Join(',', materialStorages.Select(e => $"{e.NodeId}"))}]");
            }
            var taskTemplate = await _taskTemplateService.FirstOrDefaultAsync(e => e.Code == "A");
            if (taskTemplate is null)
            {
                return BadRequest($"任务模板不存在，任务模板编码 [A]");
            }
            var taskTemplateProcesses = await _taskTemplateProcessService.ToListAsync(e => e.TaskTemplateId == taskTemplate.Id);
            if (taskTemplateProcesses is null || taskTemplateProcesses.Count == 0)
            {
                return BadRequest($"任务模板子任务不存在，任务模板子任务编码");
            }

            var nodeCodes = materialStorageNodes.Select(e => e.Code);
            var taskInstanceNodes = taskTemplateProcesses.OrderBy(e => e.SortNumber).Select((e, i) => i == 0 || i == taskTemplateProcesses.Count - 1 ? new KeyValuePair<string, string?>(e.Code, "#") : nodeCodes.Contains(e.NodeCode) ? new KeyValuePair<string, string?>(e.Code, e.NodeCode) : new KeyValuePair<string, string?>(e.Code, "-"));
            var taskInstance = new TaskInstanceDto
            {
                TaskTemplateId = taskTemplate.Id,
                Code = $"{containerStorageNode.Code}=>[{string.Join(',', taskInstanceNodes.Where(e => e.Value != "-").Select(e => $"{e.Key}"))}]",
                Name = $"站点 [{containerStorageNode.Code}] 去目标站点 [{string.Join(',', taskInstanceNodes.Where(e => e.Value != "-").Select(e => $"{e.Key}"))}]",
                Type = TaskInstanceConst.Type.Normal,
                State = TaskInstanceConst.State.Released,
                Nodes = taskInstanceNodes.Select(e => e.Value)?.ToArray()
            };
            await _taskInstanceService.AddAsync(taskInstance);

            work.TaskId = taskInstance.Id;
            work.TaskCode = taskInstance.Code;
            work.State = taskInstance.State;
            await _workService.UpdateAsync(work);

            return Ok();
        }
    }
}