using Common.Frame.Services.Frame.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Extends.Flow;
using FASS.Service.Models.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc.Extensions;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;
using FASS.Web.Api.Models.Pc;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-PDA缺料呼叫")]
    public class WarehouseWorkPdaCallController : BaseController
    {
        private readonly ILogger<WarehouseWorkPdaCallController> _logger;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly IWorkService _workService;
        private readonly ITaskRecordService _taskRecordService;
        //private readonly INodeService _nodeService;
        private readonly ITaskTemplateService _taskTemplateService;
        private readonly ITaskTemplateProcessService _taskTemplateProcessService;
        //private readonly ITaskInstanceService _taskInstanceService;
        //private readonly ITaskInstanceProcessService _taskInstanceProcessService;
        //private readonly ITaskInstanceActionService _taskInstanceActionService;
        private readonly IDictItemService _dictItemService;
        private readonly IAreaService _areaService;
        private readonly ICarService _carService;
        private readonly ICarTypeService _carTypeService;
        private readonly IPreMaterialService _preMaterialService;
        private readonly IPreWorkService _preWorkService;
        private readonly AppSettings _appSettings;

        public WarehouseWorkPdaCallController(
            ILogger<WarehouseWorkPdaCallController> logger,
            IStorageService storageService,
            IContainerService containerService,
            IMaterialService materialService,
            IWorkService workService,
            ITaskRecordService taskRecordService,
            //INodeService nodeService,
            ITaskTemplateService taskTemplateService,
            ITaskTemplateProcessService taskTemplateProcessService,
            //ITaskInstanceService taskInstanceService,
            //ITaskInstanceProcessService taskInstanceProcessService,
            //ITaskInstanceActionService taskInstanceActionService,
            IDictItemService dictValueService,
            IAreaService areaService,
            ICarService carService,
            ICarTypeService carTypeService,
            IPreMaterialService preMaterialService,
            IPreWorkService preWorkService,
            AppSettings appSettings)
        {
            _logger = logger;
            _storageService = storageService;
            _containerService = containerService;
            _materialService = materialService;
            _workService = workService;
            _taskRecordService = taskRecordService;
            //_nodeService = nodeService;
            _taskTemplateService = taskTemplateService;
            _taskTemplateProcessService = taskTemplateProcessService;
            //_taskInstanceService = taskInstanceService;
            //_taskInstanceProcessService = taskInstanceProcessService;
            //_taskInstanceActionService = taskInstanceActionService;
            _dictItemService = dictValueService;
            _areaService = areaService;
            _carService = carService;
            _carTypeService = carTypeService;
            _preMaterialService = preMaterialService;
            _preWorkService = preWorkService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _workService.ToPageAsync(param);
            var areaDtos = await _areaService.Set().Where(e => e.IsEnable).ToListAsync();
            var storageDtos = await _storageService.Set().Where(e => e.IsEnable).ToListAsync();
            var taskIds = page.Data.Select(e => e.TaskId);
            var taskRecordDtos = await _taskRecordService.Set().Where(e => taskIds.Contains(e.Id)).ToListAsync();
            var carDtos = await _carService.Set().ToListAsync();
            for (var i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].AreaName = areaDtos.Where(e => e.Id == page.Data[i].AreaId).FirstOrDefault()?.Name;
                //根据任务id获取任务记录以及任务起点和终点库位名称
                var task = taskRecordDtos.Where(e => e.Id == page.Data[i].TaskId).FirstOrDefault();
                if (task != null)
                {
                    page.Data[i].SrcStorageName = storageDtos.Where(e => e.NodeCode == task.SrcNodeCode).FirstOrDefault()?.Name;
                    page.Data[i].DestStorageName = storageDtos.Where(e => e.NodeCode == task.DestNodeCode).FirstOrDefault()?.Name;
                    page.Data[i].CarName = carDtos.Where(e => e.Id == task.CarId).FirstOrDefault()?.Name;
                }
            }
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

        [HttpGet]
        public async Task<IActionResult> GetDictItemListToSelect([FromQuery] string dictCode, [FromQuery] string param)
        {
            var data = await _dictItemService
                .Set()
                .Where(e => e.IsEnable && e.Dict.Code == dictCode
                   && (!string.IsNullOrWhiteSpace(param) ? (e.Param != null && e.Param.ToUpper().Contains(param.ToUpper())) : true))
                .OrderBy(e => e.SortNumber)
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterialListToSelect()
        {
            /*
             * 满桶周转区/满桶缓存区库位容器物料信息
             * 
             */
            //库位
            var storageDtos = await _storageService.ToListAsync(e => e.IsLock == false
                && e.IsEnable
                && (e.AreaCode == "MTZZQ" || e.AreaCode == "MTHCQ")
                && e.State == StorageConst.State.FullContainer);
            //容器
            var containerDtos = await _storageService.GetContainersAsync(storageDtos);
            //已经被预定的物料
            var materialDtos = await _preWorkService.ToListAsync(e => e.State != WorkConst.State.Completed);
            var materialCodes = materialDtos.Select(e => e.MaterialCode);
            //物料[满桶周转区物料]  --需要移除被预定的物料
            var data = (await _containerService.GetMaterialsAsync(containerDtos)).Where(e => materialCodes is null ? true : !materialCodes.Contains(e.Code)).Select(e =>
            {
                return new MaterialInof { Id = e.Id, Code = e.Code, Name = e.Name };
            });
            //未被预定的预生产物料
            var preData = (await _preMaterialService.ToListAsync(e => !e.IsLock && e.IsEnable)).Select(e =>
            {
                return new MaterialInof { Id = $"pre_{e.Id}", Code = e.Code, Name = $"{e.Name}[离心中]" };
            });
            return Ok(data.Concat(preData));
        }

        [HttpPut]
        public async Task<IActionResult> AddWorkOld([FromQuery] string areaId, [FromQuery] string storageId, [FromQuery] string callMode, [FromQuery] string materialId, [FromQuery] string carTypeId)
        {
            //判断车型是否存在
            CarTypeDto? carTypeDto = null;
            if (!string.IsNullOrEmpty(carTypeId))
            {
                carTypeDto = await _carTypeService.FirstOrDefaultAsync(e => e.Id == carTypeId);
                if (carTypeDto == null) return BadRequest($"获取车辆类型失败 [{carTypeId}]");
            }
            //判断叫料区域是否存在
            var area = await _areaService.FirstOrDefaultAsync(e => e.Id == areaId);
            if (area == null)
            {
                return BadRequest($"叫料区域不存在,区域编号 [{areaId}]");
            }
            //判断叫料库位是否存在
            var storage = await _storageService.Set().FirstOrDefaultAsync(e => e.Id == storageId);
            if (storage == null)
            {
                return BadRequest($"库位不存在,库位编号 [{storageId}]");
            }
            //判定物料类型(满桶周转区物料、预生产物料)
            var materialMode = (!string.IsNullOrEmpty(materialId) && materialId.Contains("pre_")) ? "pre" : "exist";
            //判断物料是否存在
            materialId = !string.IsNullOrEmpty(materialId) ? materialId.Replace("pre_", "") : materialId;//获取原始物料id
            var dictItem = await _dictItemService.FirstOrDefaultAsync(e => e.Code == callMode && e.IsEnable);
            if (dictItem == null)
            {
                return BadRequest($"呼叫模式不存在 [{callMode}]");
            }
            var taskTemplate = await _taskTemplateService.FirstOrDefaultAsync(e => e.Code == "Double");
            if (taskTemplate == null)
            {
                return BadRequest($"任务模板不存在，任务模板编码 [Double]");
            }
            var taskTemplateProcesses = await _taskTemplateProcessService.ToListAsync(e => e.TaskTemplateId == taskTemplate.Id);
            if (taskTemplateProcesses == null || taskTemplateProcesses.Count == 0)
            {
                return BadRequest($"任务模板编码 [Double] 子任务不存在");
            }
            TaskRecordDto taskRecordDto;
            var callModeName = $"{storage.Name}-{dictItem.Name}";
            if (callMode == "EmptyOffline")
            {
                if (storage.IsLock)
                {
                    return BadRequest($"空桶下线失败，叫料工位已被锁定");
                }
                if (storage.State == StorageConst.State.NoneContainer)
                {
                    return BadRequest($"空桶下线失败，叫料工位无料桶");
                }

                //1、下空桶时，优先放置到空桶缓存位，空桶缓存位不存在时，放置到空桶周转区
                var storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.IsEnable && e.AreaCode != null && e.AreaCode.Contains("KTHCQ")).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//找左侧空桶缓存区和右侧空桶缓存区
                if (storageEmpty == null)
                {
                    //先判定是否存在空桶中转位,且为未锁定状态
                    storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "KTZZQ" && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//放空桶放里层，取SortNumber小的库位
                }
                if (storageEmpty == null)
                {
                    return BadRequest($"空桶下线失败，不存在空桶中转区库位");
                }
                //2、下发点对点搬运任务
                taskRecordDto = CarTaskExtension.CreateTaskRecord(taskTemplate.Id, storage, storageEmpty);
                if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
                var containers = await _storageService.GetContainersAsync(storage);
                if (containers == null || containers.Count() == 0)
                {
                    return BadRequest($"起点工位[{storage.Name}]没有容器，任务下发失败");
                }
                //3、锁定起始库位和终点库位
                storage.IsLock = true;
                storageEmpty.IsLock = true;
                //await _storageService.UpdateAsync(new List<StorageDto> { storage, storageEmpty });
                //4、空桶下线时，先清掉桶内物料信息.重置桶的状态
                var container = containers.First();
                var materials = await _containerService.GetMaterialsAsync(container);
                if (materials != null && materials.Count() > 0)
                    await _containerService.MaterialDeleteAsync(container.Id, materials);
                container.State = ContainerConst.State.EmptyMaterial;
                //await _containerService.UpdateAsync(container);
                await _containerService.Repository.ExecuteUpdateAsync(e => e.Id == container.Id, s => s.SetProperty(b => b.State, container.State));
                taskRecordDto.Extend = new TaskRecordExtend
                {
                    ContainerSize = new Service.Models.FlowExtend.ContainerSize
                    {
                        Width = container.Width,
                        Height = container.Height,
                        Length = container.Length
                    },
                    Material = string.Empty
                }.ToJson();//扩展容器和物料
                //5、添加work记录
                var result = await _workService.AddWorkAsync(areaId, callModeName, taskRecordDto, container, "");
                if (result == 0)
                {
                    return BadRequest($"空桶下线任务提交失败");
                }
                //6、自动发布任务
                var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                try
                {
                    var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
                    var containerSize = CarTaskExtension.ToContainerSize(container.Length, container.Width, container.Height);
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                    };
                    var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                        //更具taskid更新work的状态
                        var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Released;
                            await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                        }
                    }
                    else
                    {
                        return BadRequest($"发布任务失败 Code[{resp.Code}], Message[{resp.Message}]");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"发布任务失败 => ex:{ex}");
                }
            }
            else if (callMode == "FullOnline")
            {
                if (materialMode == "pre")
                {
                    //预定 写入预执行任务列表、更新预生产物料状态锁
                    //1、获取预叫物料信息
                    var preMaterialDto = await _preMaterialService.FirstOrDefaultAsync(e => e.Id == materialId && e.IsLock == false);
                    if (preMaterialDto is null)
                    {
                        return BadRequest($"物料不存在,物料编号 [{materialId}]");
                    }
                    //2、检查相同物料是否被重复预定
                    if (await _preWorkService.AnyAsync(e => e.MaterialCode == preMaterialDto.Code))
                    {
                        return BadRequest($"物料已被预定创建预定任务失败,物料编号 [{preMaterialDto.Code}]");
                    }
                    //3、写入预定任务表
                    await _preWorkService.AddAsync(new Service.Dtos.Warehouse.PreWorkDto
                    {
                        Code = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        Name = $"起点 [{preMaterialDto.StorageName}] 到终点[{storage.Name}],物料[{preMaterialDto.Code}]",
                        SrcStorageId = preMaterialDto.StorageId!,//物料生产工位
                        DestStorageId = storageId,//当前工位
                        MaterialCode = preMaterialDto.Code,
                        Type = WorkConst.Type.Normal,
                        State = WorkConst.State.Created
                    });
                    //4、更新预定物料锁状态
                    await _preMaterialService.Repository.ExecuteUpdateAsync(e => e.Id == materialId, s => s.SetProperty(b => b.IsLock, true));
                    return Ok();
                }
                var material = await _materialService.FirstOrDefaultAsync(e => e.Id == materialId);
                if (storage.IsLock)
                {
                    return BadRequest($"满桶上线失败，叫料工位已被锁定");
                }
                if (storage.State != StorageConst.State.NoneContainer)
                {
                    return BadRequest($"满桶上线失败，叫料工位已有料桶");
                }
                if (material == null)
                    return BadRequest($"物料不存在,物料编号 [{materialId}]");
                //1、获取容器
                var containers = await _materialService.GetContainersAsync(material);
                if (containers == null || containers.Count() == 0)
                    return BadRequest($"物料所在容器不存在,物料编号 [{materialId}]");
                var container = containers.First();
                //2、获取库位
                var storages = await _containerService.GetStoragesAsync(container);
                if (storages == null || storages.Count() == 0)
                    return BadRequest($"物料库位不存在,物料编号 [{materialId}]");
                var storageSrc = storages.First();
                if (storageSrc.IsLock)
                {
                    return BadRequest($"满桶上线失败，所需物料工位已被锁定");
                }
                //3、锁定起始库位和终点库位
                storage.IsLock = true;
                storageSrc.IsLock = true;
                //await _storageService.UpdateAsync(new List<StorageDto> { storage, storageSrc });
                //4、下发点对点搬运任务
                taskRecordDto = CarTaskExtension.CreateTaskRecord(taskTemplate.Id, storageSrc, storage);
                if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
                taskRecordDto.Extend = new TaskRecordExtend
                {
                    ContainerSize = new Service.Models.FlowExtend.ContainerSize
                    {
                        Width = container.Width,
                        Height = container.Height,
                        Length = container.Length
                    },
                    Material = material.Code
                }.ToJson();
                //5、添加work记录
                var result = await _workService.AddWorkAsync(areaId, callModeName, taskRecordDto, container, material.Code);
                if (result == 0)
                {
                    return BadRequest($"满桶上线任务提交失败");
                }
                //6、自动发布任务
                var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                try
                {
                    var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
                    var containerSize = CarTaskExtension.ToContainerSize(container.Length, container.Width, container.Height);
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                    };
                    var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                        //更具taskid更新work的状态
                        var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Released;
                            await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                        }
                    }
                    else
                    {
                        return BadRequest($"发布任务失败 Code[{resp.Code}], Message[{resp.Message}]");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"发布任务失败 => ex:{ex}");
                }
            }
            else
            {
                //callMode == "EmptyFullExchange"
                if (materialMode == "pre")
                {
                    return BadRequest($"空满桶交换联动任务失败，离心中的物料不支持联动任务!");
                }
                //是否存在空桶中转位，且存在满桶
                if (storage.IsLock)
                {
                    return BadRequest($"空满桶交换联动任务失败，叫料工位已被锁定");
                }
                var material = await _materialService.FirstOrDefaultAsync(e => e.Id == materialId);
                if (material == null)
                    return BadRequest($"物料不存在,物料编号 [{materialId}]");
                var containers = await _materialService.GetContainersAsync(material);
                if (containers == null || containers.Count() == 0)
                    return BadRequest($"物料所在容器不存在,物料编号 [{materialId}]");
                var container = containers.First();
                var storages = await _containerService.GetStoragesAsync(container);
                if (storages == null || storages.Count() == 0)
                    return BadRequest($"物料库位不存在,物料编号 [{materialId}]");
                //1、获取满桶工位
                var storageFull = storages.First();
                if (storageFull.IsLock)
                {
                    return BadRequest($"满桶工位已被锁定");
                }
                //2、获取空桶工位
                var storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "KTZZQ" && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                if (storageEmpty == null)
                {
                    return BadRequest($"不存在空桶中转区库位");
                }

                /*
                 * 叫料库位storage
                 * 放空库位storageEmpty
                 * 取满库位storageSrc
                 */
                //3、开始锁定库位
                var emptyContainers = await _storageService.GetContainersAsync(storage);
                if (emptyContainers == null || emptyContainers.Count() == 0)
                {
                    return BadRequest($"叫料工位[{storage.Name}]没有容器，任务下发失败");
                }
                storage.IsLock = true;
                storageFull.IsLock = true;
                storageEmpty.IsLock = true;
                //await _storageService.UpdateAsync(new List<StorageDto> { storage, storageFull, storageEmpty });
                //4、下发点对点搬运任务
                taskRecordDto = CarTaskExtension.CreateTaskRecord(taskTemplate.Id, storage, storageEmpty);//取空任务
                taskRecordDto.Code = $"{taskRecordDto.Code}-1";
                taskRecordDto.Priority = 1;//第一段任务优先级设置为1
                if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
                var followupTaskDto = CarTaskExtension.CreateTaskRecord(taskTemplate.Id, storageFull, storage);//后续送满任务
                followupTaskDto.Code = $"{taskRecordDto.Code.Replace("-1", "-2")}";
                if (carTypeDto != null) followupTaskDto.CarTypeId = carTypeDto.Id;
                //5、添加work记录
                //1)取空桶
                //空桶下线时，先清掉桶内物料信息.重置桶的状态
                var emptyContainer = emptyContainers.First();
                //将后置任务记录id存入前置任务Extend字段
                taskRecordDto.Extend = new TaskRecordExtend
                {
                    FollowUpTaskId = followupTaskDto.Id,
                    ContainerSize = new ContainerSize
                    {
                        Width = emptyContainer.Width,
                        Height = emptyContainer.Height,
                        Length = emptyContainer.Length
                    },
                    Material = ""
                }.ToJson();
                var materials = await _containerService.GetMaterialsAsync(emptyContainer);
                if (materials != null && materials.Count() > 0)
                    await _containerService.MaterialDeleteAsync(emptyContainer.Id, materials);
                emptyContainer.State = ContainerConst.State.EmptyMaterial;
                //await _containerService.UpdateAsync(emptyContainer);
                await _containerService.Repository.ExecuteUpdateAsync(e => e.Id == emptyContainer.Id, s => s.SetProperty(b => b.State, ContainerConst.State.EmptyMaterial));
                var result = await _workService.AddWorkAsync(areaId, callModeName + "(下空桶)", taskRecordDto, emptyContainer, "", "EmptyFullExchange");
                if (result == 0)
                {
                    return BadRequest($"空满桶交换任务-取空桶提交失败");
                }
                //2)送满桶
                followupTaskDto.Extend = new TaskRecordExtend
                {
                    ContainerSize = new ContainerSize
                    {
                        Width = container.Width,
                        Height = container.Height,
                        Length = container.Length
                    },
                    Material = material.Code,
                    Priority = 1
                }.ToJson();
                var result2 = await _workService.AddWorkAsync(areaId, callModeName + "(上满桶)", followupTaskDto, container, material.Code);
                if (result2 == 0)
                {
                    return BadRequest($"空满桶交换任务-送满桶提交失败");
                }

                //6、自动发布任务
                var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                try
                {
                    var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
                    var containerSize = CarTaskExtension.ToContainerSize(emptyContainer.Length, emptyContainer.Width, emptyContainer.Height);
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                    };
                    var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                        //更具taskid更新work的状态
                        var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Released;
                            await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                        }
                    }
                    else
                    {
                        return BadRequest($"发布任务失败 Code[{resp.Code}], Message[{resp.Message}]");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"发布任务失败 => ex:{ex}");
                }
            }
            return Ok();
        }

        /// <summary>
        /// Pda上满桶预定模式
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AddWork([FromQuery] string areaId, [FromQuery] string storageId, [FromQuery] string callMode, [FromQuery] string materialId, [FromQuery] string carTypeId)
        {
            //判断车型是否存在
            CarTypeDto? carTypeDto = null;
            if (!string.IsNullOrEmpty(carTypeId))
            {
                carTypeDto = await _carTypeService.FirstOrDefaultAsync(e => e.Id == carTypeId);
                if (carTypeDto == null) return BadRequest($"获取车辆类型失败 [{carTypeId}]");
            }
            //判断叫料区域是否存在
            var area = await _areaService.FirstOrDefaultAsync(e => e.Id == areaId);
            if (area == null)
            {
                return BadRequest($"叫料区域不存在,区域编号 [{areaId}]");
            }
            //判断叫料库位是否存在
            var storage = await _storageService.Set().FirstOrDefaultAsync(e => e.Id == storageId);
            if (storage == null)
            {
                return BadRequest($"库位不存在,库位编号 [{storageId}]");
            }
            //判定物料类型(满桶周转区物料、预生产物料)
            var materialMode = (!string.IsNullOrEmpty(materialId) && materialId.Contains("pre_")) ? "pre" : "exist";
            //判断物料是否存在
            materialId = !string.IsNullOrEmpty(materialId) ? materialId.Replace("pre_", "") : materialId;//获取原始物料id
            var dictItem = await _dictItemService.FirstOrDefaultAsync(e => e.Code == callMode && e.IsEnable);
            if (dictItem == null)
            {
                return BadRequest($"呼叫模式不存在 [{callMode}]");
            }
            var taskTemplate = await _taskTemplateService.FirstOrDefaultAsync(e => e.Code == "Double");
            if (taskTemplate == null)
            {
                return BadRequest($"任务模板不存在，任务模板编码 [Double]");
            }
            var taskTemplateProcesses = await _taskTemplateProcessService.ToListAsync(e => e.TaskTemplateId == taskTemplate.Id);
            if (taskTemplateProcesses == null || taskTemplateProcesses.Count == 0)
            {
                return BadRequest($"任务模板编码 [Double] 子任务不存在");
            }
            TaskRecordDto taskRecordDto;
            var callModeName = $"{storage.Name}-{dictItem.Name}";
            if (callMode == "EmptyOffline")
            {
                if (storage.IsLock)
                {
                    return BadRequest($"空桶下线失败，叫料工位已被锁定");
                }
                if (storage.State == StorageConst.State.NoneContainer)
                {
                    return BadRequest($"空桶下线失败，叫料工位无料桶");
                }

                //1、下空桶时，优先放置到空桶缓存位，空桶缓存位不存在时，放置到空桶周转区
                var storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.IsEnable && e.AreaCode != null && e.AreaCode.Contains("KTHCQ")).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//找左侧空桶缓存区和右侧空桶缓存区
                if (storageEmpty == null)
                {
                    //先判定是否存在空桶中转位,且为未锁定状态
                    storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "KTZZQ" && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//放空桶放里层，取SortNumber小的库位
                }
                if (storageEmpty == null)
                {
                    return BadRequest($"空桶下线失败，不存在空桶中转区库位");
                }
                //2、下发点对点搬运任务
                taskRecordDto = CarTaskExtension.CreateTaskRecord(taskTemplate.Id, storage, storageEmpty);
                if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
                var containers = await _storageService.GetContainersAsync(storage);
                if (containers == null || containers.Count() == 0)
                {
                    return BadRequest($"起点工位[{storage.Name}]没有容器，任务下发失败");
                }
                //3、锁定起始库位和终点库位
                storage.IsLock = true;
                storageEmpty.IsLock = true;
                //await _storageService.UpdateAsync(new List<StorageDto> { storage, storageEmpty });
                //4、空桶下线时，先清掉桶内物料信息.重置桶的状态
                var container = containers.First();
                var materials = await _containerService.GetMaterialsAsync(container);
                if (materials != null && materials.Count() > 0)
                    await _containerService.MaterialDeleteAsync(container.Id, materials);
                container.State = ContainerConst.State.EmptyMaterial;
                //await _containerService.UpdateAsync(container);
                await _containerService.Repository.ExecuteUpdateAsync(e => e.Id == container.Id, s => s.SetProperty(b => b.State, container.State));
                taskRecordDto.Extend = new TaskRecordExtend
                {
                    ContainerSize = new Service.Models.FlowExtend.ContainerSize
                    {
                        Width = container.Width,
                        Height = container.Height,
                        Length = container.Length
                    },
                    Material = string.Empty
                }.ToJson();//扩展容器和物料
                //5、添加work记录
                var result = await _workService.AddWorkAsync(areaId, callModeName, taskRecordDto, container, "");
                if (result == 0)
                {
                    return BadRequest($"空桶下线任务提交失败");
                }
                //6、自动发布任务
                var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                try
                {
                    var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
                    var containerSize = CarTaskExtension.ToContainerSize(container.Length, container.Width, container.Height);
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                    };
                    var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                        //更具taskid更新work的状态
                        var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Released;
                            await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                        }
                    }
                    else
                    {
                        return BadRequest($"发布任务失败 Code[{resp.Code}], Message[{resp.Message}]");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"发布任务失败 => ex:{ex}");
                }
            }
            else if (callMode == "FullOnline")
            {
                if (materialMode == "pre")
                {
                    //预定 写入预执行任务列表、更新预生产物料状态锁
                    //1、获取预叫物料信息
                    var preMaterialDto = await _preMaterialService.FirstOrDefaultAsync(e => e.Id == materialId && e.IsLock == false);
                    if (preMaterialDto is null)
                    {
                        return BadRequest($"物料不存在,物料编号 [{materialId}]");
                    }
                    //2、检查相同物料是否被重复预定
                    if (await _preWorkService.AnyAsync(e => e.MaterialCode == preMaterialDto.Code))
                    {
                        return BadRequest($"物料已被预定创建预定任务失败,物料编号 [{preMaterialDto.Code}]");
                    }
                    //3、写入预定任务表
                    await _preWorkService.AddAsync(new Service.Dtos.Warehouse.PreWorkDto
                    {
                        Code = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        Name = $"起点 [{preMaterialDto.StorageName}] 到终点[{storage.Name}],物料[{preMaterialDto.Code}]",
                        SrcStorageId = preMaterialDto.StorageId!,//物料生产工位
                        DestStorageId = storageId,//当前工位
                        MaterialCode = preMaterialDto.Code,
                        Type = WorkConst.Type.Normal,
                        State = WorkConst.State.Created
                    });
                    //4、更新预定物料锁状态
                    await _preMaterialService.Repository.ExecuteUpdateAsync(e => e.Id == materialId, s => s.SetProperty(b => b.IsLock, true));
                    return Ok();
                }
                var material = await _materialService.FirstOrDefaultAsync(e => e.Id == materialId);
                if (material == null)
                    return BadRequest($"物料不存在,物料编号 [{materialId}]");
                //1、获取容器
                var containers = await _materialService.GetContainersAsync(material);
                if (containers == null || containers.Count() == 0)
                    return BadRequest($"物料所在容器不存在,物料编号 [{materialId}]");
                var container = containers.First();
                //2、获取库位
                var storages = await _containerService.GetStoragesAsync(container);
                if (storages == null || storages.Count() == 0)
                    return BadRequest($"物料库位不存在,物料编号 [{materialId}]");
                var storageSrc = storages.First();
                if (storageSrc.IsLock)
                {
                    return BadRequest($"满桶上线失败，所需物料工位已被锁定");
                }
                if (storage.IsLock || storage.State != StorageConst.State.NoneContainer)
                {
                    //叫料工位不符合条件，改为预定物料模式
                    //1、检查相同物料是否被重复预定
                    if (await _preWorkService.AnyAsync(e => e.MaterialCode == material.Code))
                    {
                        return BadRequest($"物料已被预定创建预定任务失败,物料编号 [{material.Code}]");
                    }
                    //2、写入预定任务表
                    await _preWorkService.AddAsync(new Service.Dtos.Warehouse.PreWorkDto
                    {
                        Code = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        Name = $"起点 [{storageSrc.Name}] 到终点[{storage.Name}],物料[{material.Code}]",
                        SrcStorageId = storageSrc.Id!,//物料生产工位
                        DestStorageId = storageId,//当前工位
                        MaterialCode = material.Code,
                        Type = WorkConst.Type.Normal,
                        State = WorkConst.State.Released
                    });
                    //4、更新预定物料锁状态
                    await _preMaterialService.Repository.ExecuteUpdateAsync(e => e.Id == materialId, s => s.SetProperty(b => b.IsLock, true));
                    return Ok();
                }
                //if (storage.IsLock)
                //{
                //    return BadRequest($"满桶上线失败，叫料工位已被锁定");
                //}
                //if (storage.State != StorageConst.State.NoneContainer)
                //{
                //    return BadRequest($"满桶上线失败，叫料工位已有料桶");
                //}
                //3、锁定起始库位和终点库位
                storage.IsLock = true;
                storageSrc.IsLock = true;
                //await _storageService.UpdateAsync(new List<StorageDto> { storage, storageSrc });
                //4、下发点对点搬运任务
                taskRecordDto = CarTaskExtension.CreateTaskRecord(taskTemplate.Id, storageSrc, storage);
                if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
                taskRecordDto.Extend = new TaskRecordExtend
                {
                    ContainerSize = new Service.Models.FlowExtend.ContainerSize
                    {
                        Width = container.Width,
                        Height = container.Height,
                        Length = container.Length
                    },
                    Material = material.Code
                }.ToJson();
                //5、添加work记录
                var result = await _workService.AddWorkAsync(areaId, callModeName, taskRecordDto, container, material.Code);
                if (result == 0)
                {
                    return BadRequest($"满桶上线任务提交失败");
                }
                //6、自动发布任务
                var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                try
                {
                    var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
                    var containerSize = CarTaskExtension.ToContainerSize(container.Length, container.Width, container.Height);
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                    };
                    var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                        //更具taskid更新work的状态
                        var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Released;
                            await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                        }
                    }
                    else
                    {
                        return BadRequest($"发布任务失败 Code[{resp.Code}], Message[{resp.Message}]");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"发布任务失败 => ex:{ex}");
                }
            }
            else
            {
                return BadRequest($"PDA呼叫任务失败，不存在呼叫模式[{callMode}]");
            }
            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _workService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _workService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _workService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _workService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _workService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _workService.DeleteAllAsync();
            return Ok();
        }

    }
}
