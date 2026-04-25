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
    [Tags("移动管理-任务区域B")]
    public class WarehouseWorkAreaBController : BaseController
    {
        private readonly ILogger<WarehouseWorkAreaBController> _logger;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly IWorkService _workService;
        private readonly INodeService _nodeService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly ITaskTemplateService _taskTemplateService;
        private readonly ITaskTemplateProcessService _taskTemplateProcessService;

        public WarehouseWorkAreaBController(
            ILogger<WarehouseWorkAreaBController> logger,
            IStorageService storageService,
            IContainerService containerService,
            IMaterialService materialService,
            IWorkService workService,
            INodeService nodeService,
            ITaskInstanceService taskInstanceService,
            ITaskTemplateService taskTemplateService,
            ITaskTemplateProcessService taskTemplateProcessService)
        {
            _logger = logger;
            _storageService = storageService;
            _containerService = containerService;
            _materialService = materialService;
            _workService = workService;
            _nodeService = nodeService;
            _taskInstanceService = taskInstanceService;
            _taskTemplateService = taskTemplateService;
            _taskTemplateProcessService = taskTemplateProcessService;
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
            if (container == null)
            {
                return BadRequest($"获取容器失败 [{container}]");
            }
            if (container.State != ContainerConst.State.EmptyMaterial || container.IsLock)
            {
                return BadRequest($"容器状态或锁定状态不正确 [{container}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage == null)
            {
                return BadRequest($"获取容器储位失败 [{containerStorage}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode == null)
            {
                return BadRequest($"获取容器储位站点失败 [{containerStorageNode}]");
            }
            var containerMaterials = await _materialService.ToListAsync(e => materialIds.Contains(e.Id));
            if (containerMaterials == null || containerMaterials.Count == 0)
            {
                return BadRequest($"获取容器物料失败 []");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 == e2).ToList();
            if (materialStorages == null || materialStorages.Count == 0)
            {
                return BadRequest($"获取物料储位失败 []");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes == null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"获取物料储位站点失败 []");
            }
            await _containerService.MaterialAddAsync(containerId, containerMaterials);
            var workAdd = new WorkDto()
            {
                ContainerId = container.Id,
                Code = $"{container.Code}=>[{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]",
                Name = $"容器 [{container.Code}] 绑定物料 [{string.Join(',', containerMaterials.Select(e => $"{e.Code}"))}]",
                Type = WorkConst.Type.Normal,
                State = WorkConst.State.Created
            };
            _workService.Add(workAdd);
            container.State = ContainerConst.State.FullMaterial;
            container.IsLock = true;
            _containerService.Update(container);
            var result = await _workService.AnyAsync(e => e.Id == workAdd.Id);
            if (!result)
            {
                return BadRequest();
            }
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
            if (container == null)
            {
                return BadRequest($"获取容器失败 [{container}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage == null)
            {
                return BadRequest($"获取容器储位失败 [{containerStorage}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode == null)
            {
                return BadRequest($"获取容器储位站点失败 [{containerStorageNode}]");
            }
            var containerMaterials = _containerService.GetMaterials(container).ToList();
            if (containerMaterials == null || containerMaterials.Count == 0)
            {
                return BadRequest($"获取容器物料失败 []");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 == e2).ToList();
            if (materialStorages == null || materialStorages.Count == 0)
            {
                return BadRequest($"获取物料储位失败 []");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes == null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"获取物料储位站点失败 []");
            }
            var storage = await _storageService.FirstOrDefaultAsync(e => e.Id == storageId);
            if (storage == null)
            {
                return BadRequest($"获取储位失败 [{storage}]");
            }
            var storageMaterials = _storageService.GetMaterials(storage).ToList();
            if (storageMaterials == null || storageMaterials.Count == 0)
            {
                return BadRequest($"获取储位物料失败 []");
            }
            await _containerService.MaterialDeleteAsync(containerId, storageMaterials);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Remove([FromQuery] string keyValue)
        {
            var work = await _workService.FirstOrDefaultAsync(e => e.Id == keyValue);
            if (work == null)
            {
                return BadRequest($"获取任务失败");
            }
            if (!WorkConst.State.Update.Contains(work.State))
            {
                return BadRequest($"任务状态不正确");
            }
            var container = await _containerService.FirstOrDefaultAsync(e => e.Id == work.ContainerId);
            if (container == null)
            {
                return BadRequest($"获取容器失败 [{container}]");
            }
            if (container.State == ContainerConst.State.EmptyMaterial || !container.IsLock)
            {
                return BadRequest($"容器状态或锁定状态不正确 [{container}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage == null)
            {
                return BadRequest($"获取容器储位失败 [{containerStorage}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode == null)
            {
                return BadRequest($"获取容器储位站点失败 [{containerStorageNode}]");
            }
            var containerMaterials = _containerService.GetMaterials(container).ToList();
            if (containerMaterials == null || containerMaterials.Count == 0)
            {
                return BadRequest($"获取容器物料失败 []");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 == e2).ToList();
            if (materialStorages == null || materialStorages.Count == 0)
            {
                return BadRequest($"获取物料储位失败 []");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes == null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"获取物料储位站点失败 []");
            }
            await _containerService.MaterialDeleteAsync(container.Id, containerMaterials);
            await _workService.DeleteAsync(work);
            container.State = ContainerConst.State.EmptyMaterial;
            container.IsLock = false;
            _containerService.Update(container);
            var result = await _workService.AnyAsync(e => e.Id == work.Id);
            if (result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Task([FromQuery] string keyValue)
        {
            var work = await _workService.FirstOrDefaultAsync(e => e.Id == keyValue);
            if (work == null)
            {
                return BadRequest($"获取任务失败");
            }
            if (!WorkConst.State.Update.Contains(work.State))
            {
                return BadRequest($"任务状态不正确");
            }
            var container = await _containerService.FirstOrDefaultAsync(e => e.Id == work.ContainerId);
            if (container == null)
            {
                return BadRequest($"获取容器失败 [{container}]");
            }
            if (container.State == ContainerConst.State.EmptyMaterial || !container.IsLock)
            {
                return BadRequest($"容器状态或锁定状态不正确 [{container}]");
            }
            var containerStorage = _containerService.GetStorages(container).FirstOrDefault();
            if (containerStorage == null)
            {
                return BadRequest($"获取容器储位失败 [{containerStorage}]");
            }
            var containerStorageNode = await _nodeService.FirstOrDefaultAsync(e => e.Id == containerStorage.NodeId);
            if (containerStorageNode == null)
            {
                return BadRequest($"获取容器储位站点失败 [{containerStorageNode}]");
            }
            var containerMaterials = _containerService.GetMaterials(container).ToList();
            if (containerMaterials == null || containerMaterials.Count == 0)
            {
                return BadRequest($"获取容器物料失败 []");
            }
            var materialStorages = _materialService.GetStorages(containerMaterials).Distinct((e1, e2) => e1 == e2).ToList();
            if (materialStorages == null || materialStorages.Count == 0)
            {
                return BadRequest($"获取物料储位失败 []");
            }
            var materialStorageNodes = await _nodeService.ToListAsync(e => materialStorages.Select(s => s.NodeId).Contains(e.Id));
            if (materialStorageNodes == null || materialStorageNodes.Count == 0)
            {
                return BadRequest($"获取物料储位站点失败 []");
            }
            var taskTemplate = await _taskTemplateService.FirstOrDefaultAsync(e => e.Code == "A");
            if (taskTemplate == null)
            {
                return BadRequest($"获取任务模板失败 [A]");
            }
            var taskTemplateProcesses = await _taskTemplateProcessService.ToListAsync(e => e.TaskTemplateId == taskTemplate.Id);
            if (taskTemplateProcesses == null || taskTemplateProcesses.Count == 0)
            {
                return BadRequest($"获取任务模板子任务失败 []");
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
                Nodes = taskInstanceNodes.Select(e => e.Value).ToArray()
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