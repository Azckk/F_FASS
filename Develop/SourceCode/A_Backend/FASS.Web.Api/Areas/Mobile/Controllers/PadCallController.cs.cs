using Common.Frame.Services.Frame.Interfaces;
using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using Common.NETCore.Models;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Data;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Extends.Flow;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc.Extensions;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{

    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-平板呼叫")]
    public class PadCallController : BaseController
    {
        private readonly ILogger<PadCallController> _logger;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly IDictItemService _dictItemService;
        private readonly IConfigService _configService;
        private readonly IAreaService _areaService;
        private readonly ICarTypeService _carTypeService;
        private readonly IWorkService _workService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly AppSettings _appSettings;

        public PadCallController(
            ILogger<PadCallController> logger,
            IStorageService storageService,
            IContainerService containerService,
            IMaterialService materialService,
            IDictItemService dictValueService,
            IConfigService configService,
            IAreaService areaService,
            ICarTypeService carTypeService,
            IWorkService workService,
            ITaskRecordService taskRecordService,
            AppSettings appSettings)
        {
            _logger = logger;
            _storageService = storageService;
            _containerService = containerService;
            _materialService = materialService;
            _dictItemService = dictValueService;
            _configService = configService;
            _areaService = areaService;
            _carTypeService = carTypeService;
            _workService = workService;
            _taskRecordService = taskRecordService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string type, [FromQuery] string? pageParam)
        {
            var storageDtos = await _storageService.Set().Where(e => !string.IsNullOrEmpty(type) ? (e.Type == type && e.IsEnable) : e.IsEnable).ToListAsync();
            var areaDtos = await _areaService.Set().Where(e => e.IsEnable).ToListAsync();
            for (var i = 0; i < storageDtos.Count; i++)
            {
                storageDtos[i].AreaName = areaDtos.Where(e => e.Id == storageDtos[i].AreaId).FirstOrDefault()?.Name;
            }
            var storageGroups = storageDtos.GroupBy(e => e.AreaCode);
            return Ok(storageGroups);
        }

        /// <summary>
        /// 获取PAD呼叫工位类型
        /// </summary>
        /// <returns></returns>
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

        [HttpPut]
        public async Task<IActionResult> AddWork([FromQuery] string storageId, [FromQuery] string callMode, [FromQuery] string materialType, [FromQuery] string carTypeId)
        {
            //判断车型是否存在
            CarTypeDto? carTypeDto = null;
            if (!string.IsNullOrEmpty(carTypeId))
            {
                carTypeDto = await _carTypeService.FirstOrDefaultAsync(e => e.Id == carTypeId);
                if (carTypeDto == null) return BadRequest($"获取车辆类型失败 [{carTypeId}]");
            }
            var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
            //判断叫料库位是否存在
            var storage = await _storageService.FirstOrDefaultAsync(e => e.Id == storageId && e.IsEnable && !string.IsNullOrEmpty(e.NodeCode));
            if (storage == null)
            {
                return BadRequest($"库位不存在,库位编号 [{storageId}]");
            }
            var dictItem = await _dictItemService.FirstOrDefaultAsync(e => e.Code == callMode && e.IsEnable && e.Dict.Code == "CommonCallMode");
            if (dictItem == null)
            {
                return BadRequest($"模板类型不存在 [{callMode}]");
            }
            //获取配置的取空/放满区域
            var recyclingArea = await _configService.FirstOrDefaultAsync(e => e.Key == "RecyclingArea");
            AreaConfig area;
            if (recyclingArea == null || recyclingArea.Value is null)
            {
                area = new AreaConfig
                {
                    FullArea = "MTZZQ",
                    EmptyArea = "KTZZQ",
                };
            }
            else
            {
                var areaObject = recyclingArea.Value.JsonTo<AreaConfig>();
                area = new AreaConfig
                {
                    FullArea = areaObject?.FullArea ?? "MTZZQ",
                    EmptyArea = areaObject?.EmptyArea ?? "KTZZQ",
                };
            }
            var callModeName = $"{storage.Name}-{dictItem.Name}";//拼接工位-模板名称
            if (callMode == "EmptyFullExchange" || callMode == "FullEmptyExchange")
            {
                //组合动作
                if (storage.IsLock)
                {
                    return BadRequest($"叫料工位已被锁定");
                }
                if (callMode == "EmptyFullExchange")
                {
                    #region 空满交换
                    //1、叫料库位是否存在容器(空/满)
                    if (storage.State == StorageConst.State.NoneContainer)
                    {
                        return BadRequest($"取空放满失败，呼叫库位无容器");
                    }
                    //2、先判定是否存在空桶中转位,且为未锁定状态
                    var storageEmpty = await _storageService.Set().AsNoTracking().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == area.EmptyArea && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (storageEmpty == null)
                    {
                        return BadRequest($"空满交换失败，不存在空桶中转区库位");
                    }
                    //3、满桶缓存区是否存在指定类型的物料,且库位未锁定
                    var storageFullDtos = await _materialService.GetStoragesByMaterialTypeAsync(area.FullArea, materialType, false);
                    var storageFull = storageFullDtos.FirstOrDefault();
                    if (storageFull == null)
                    {
                        return BadRequest($"空满交换失败，满桶中转区不存在指定物料满桶");
                    }
                    if (storageFull.State != StorageConst.State.FullContainer)
                    {
                        return BadRequest($"取空放满失败，取满库位{storageFull.Code}无满容器");
                    }
                    var fullContainers = await _storageService.GetContainersAsync(storageFull);
                    if (fullContainers == null || fullContainers.Count() == 0)
                    {
                        return BadRequest($"取满库位[{storageFull.Code}]没有容器，任务下发失败");
                    }
                    var fullContainer = fullContainers.First();
                    if (fullContainer.State != ContainerConst.State.FullMaterial)
                    {
                        return BadRequest($"取满库位[{storageFull.Name}]容器不为满容器，任务下发失败");
                    }
                    var materialDtos = await _containerService.GetMaterialsAsync(fullContainer);
                    //4、叫料库位容器是否是空容器
                    var emptyContainers = await _storageService.GetContainersAsync(storage);
                    if (emptyContainers == null || emptyContainers.Count() == 0)
                    {
                        return BadRequest($"呼叫库位[{storage.Name}]没有容器，任务下发失败");
                    }
                    var emptyContainer = emptyContainers.First();
                    //5、空桶存在物料时，删除物料信息并更新容器状态为空容器
                    var materials = await _containerService.GetMaterialsAsync(emptyContainer);
                    if (materials != null && materials.Count() > 0)
                        await _containerService.MaterialDeleteAsync(emptyContainer.Id, materials);
                    emptyContainer.State = ContainerConst.State.EmptyMaterial;
                    await _containerService.Repository.ExecuteUpdateAsync(e => e.Id == emptyContainer.Id, s => s.SetProperty(b => b.State, ContainerConst.State.EmptyMaterial));

                    //6、下发点对点搬运任务
                    var taskRecordNewDto = CarTaskExtension.CreateTaskRecord("Double", storage, storageEmpty); //取空任务
                    taskRecordNewDto.CallMode = callMode;
                    taskRecordNewDto.Code = $"{taskRecordNewDto.Code}-1";
                    taskRecordNewDto.Priority = 1;
                    if (carTypeDto != null)
                        taskRecordNewDto.CarTypeId = carTypeDto.Id;
                    var followupTaskDto = CarTaskExtension.CreateTaskRecord("Double", storageFull, storage);//送满任务
                    followupTaskDto.Code = $"{taskRecordNewDto.Code.Replace("-1", "-2")}";
                    if (carTypeDto != null)
                        followupTaskDto.CarTypeId = carTypeDto.Id;
                    //7、将后置任务记录id存入前置任务Extend字段
                    taskRecordNewDto.Extend = new TaskRecordExtend
                    {
                        FollowUpTaskId = followupTaskDto.Id,
                        ContainerSize = new Service.Models.FlowExtend.ContainerSize
                        {
                            Width = emptyContainer.Width,
                            Height = emptyContainer.Height,
                            Length = emptyContainer.Length
                        },
                        Material = ""
                    }.ToJson();
                    //8、添加work记录
                    //1)取空桶
                    var result = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(下空桶)", taskRecordNewDto, emptyContainer, "", "EmptyFullExchange");
                    if (result == 0)
                    {
                        return BadRequest($"空满桶交换任务-取空桶提交失败");
                    }
                    //2)送满桶
                    var fullMaterials = string.Join(",", materialDtos.Select(e => e.Code));
                    followupTaskDto.Extend = new TaskRecordExtend
                    {
                        ContainerSize = new Service.Models.FlowExtend.ContainerSize
                        {
                            Width = fullContainer.Width,
                            Height = fullContainer.Height,
                            Length = fullContainer.Length
                        },
                        Material = fullMaterials
                    }.ToJson();
                    var result2 = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(上满桶)", followupTaskDto, fullContainer, fullMaterials);
                    if (result2 == 0)
                    {
                        return BadRequest($"空满桶交换任务-送满桶提交失败");
                    }
                    //9、自动发布任务
                    var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordNewDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                    try
                    {
                        var containerSize = new Service.Models.FlowExtend.ContainerSize()
                        {
                            Width = emptyContainer.Width,
                            Length = emptyContainer.Length,
                            Height = emptyContainer.Height
                        };
                        var startNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(taskRecordNewDto.SrcNodeCode) };
                        var endNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(taskRecordNewDto.DestNodeCode) };
                        var req = CarTaskExtension.ToCarTask(taskRecordNewDto, startNode, endNode, containerSize, null, carTypeCode);
                        var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                            $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                        );
                        if (resp.Code == 200)
                        {
                            dto.State = TaskInstanceConst.State.Released;
                            await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                            //根据taskid更新work的状态
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
                    #endregion
                }
                else
                {
                    #region 满空交换
                    if (storage.State != StorageConst.State.FullContainer)
                    {
                        return BadRequest($"叫料工位无满容器");
                    }
                    //1、叫料工位是否满料
                    var fullContainers = await _storageService.GetContainersAsync(storage);
                    if (fullContainers == null || fullContainers.Count() == 0)
                    {
                        return BadRequest($"叫料工位[{storage.Name}]没有容器，任务下发失败");
                    }
                    var container = fullContainers.First();//获取满容器
                    if (container.State != ContainerConst.State.FullMaterial)
                    {
                        return BadRequest($"叫料工位[{storage.Name}]容器不为满料，任务下发失败");
                    }
                    var materialDtos = await _containerService.GetMaterialsAsync(container);
                    //2、判定满桶中转区有库位且为未锁定状态
                    var storageFull = await _storageService.Set().AsNoTracking().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == area.FullArea).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (storageFull == null)
                    {
                        return BadRequest($"满桶周转区不存在空库位，任务下发失败");
                    }
                    //3、判断空桶周转区是否有空桶且为未锁定状态
                    var storageEmpty = await _storageService.Set().AsNoTracking().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsLock == false && e.AreaCode == area.EmptyArea).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (storageEmpty == null)
                    {
                        return BadRequest($"空桶周转区不存在空桶，任务下发失败");
                    }
                    var emptyContainers = await _storageService.GetContainersAsync(storageEmpty);
                    if (emptyContainers == null || emptyContainers.Count() == 0)
                    {
                        return BadRequest($"空桶周转区库位[{storageEmpty.Name}]没有容器，任务下发失败");
                    }
                    var emptyContainer = emptyContainers.First();
                    if (emptyContainer.State != ContainerConst.State.EmptyMaterial)
                    {
                        return BadRequest($"空桶周转区库位[{storage.Name}]容器不为空容器，任务下发失败");
                    }
                    //4、下发点对点搬运任务
                    var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", storage, storageFull); //取满任务
                    taskRecordDto.CallMode = callMode;
                    taskRecordDto.Code = $"{taskRecordDto.Code}-1";
                    taskRecordDto.Priority = 1;//第1段任务优先级设置为1
                    if (carTypeDto != null)
                        taskRecordDto.CarTypeId = carTypeDto.Id;
                    var followupTaskDto = CarTaskExtension.CreateTaskRecord("Double", storageEmpty, storage);//送空任务
                    followupTaskDto.Code = $"{taskRecordDto.Code.Replace("-1", "-2")}";
                    if (carTypeDto != null)
                        followupTaskDto.CarTypeId = carTypeDto.Id;
                    //5、将后置任务记录id存入前置任务Extend字段
                    var materials = string.Join(",", materialDtos.Select(e => e.Code));
                    taskRecordDto.Extend = new TaskRecordExtend
                    {
                        FollowUpTaskId = followupTaskDto.Id,
                        ContainerSize = new Service.Models.FlowExtend.ContainerSize
                        {
                            Width = container.Width,
                            Height = container.Height,
                            Length = container.Length
                        },
                        Material = materials
                    }.ToJson();
                    //6、添加work记录
                    //1)取满桶
                    var result = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(下满桶)", taskRecordDto, container, materials, "FullEmptyExchange");
                    if (result == 0)
                    {
                        return BadRequest($"满空桶交换任务-取满桶提交失败");
                    }
                    //2)送空桶
                    followupTaskDto.Extend = new TaskRecordExtend
                    {
                        ContainerSize = new Service.Models.FlowExtend.ContainerSize
                        {
                            Width = emptyContainer.Width,
                            Height = emptyContainer.Height,
                            Length = emptyContainer.Length
                        }
                    }.ToJson();
                    var result2 = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(上空桶)", followupTaskDto, emptyContainer, "");
                    if (result2 == 0)
                    {
                        return BadRequest($"满空桶交换任务-送空桶提交失败");
                    }

                    //9、自动发布任务
                    var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                    try
                    {
                        var containerSize = new Service.Models.FlowExtend.ContainerSize()
                        {
                            Width = container.Width,
                            Length = container.Length,
                            Height = container.Height
                        };
                        var startNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(taskRecordDto.SrcNodeCode) };
                        var endNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(taskRecordDto.DestNodeCode) };
                        var req = CarTaskExtension.ToCarTask(taskRecordDto, startNode, endNode, containerSize, null, carTypeCode);
                        var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                            $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                        );
                        if (resp.Code == 200)
                        {
                            dto.State = TaskInstanceConst.State.Released;
                            await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                            //根据taskid更新work的状态
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

                    #endregion
                }
            }
            else
            {
                //单步动作
                StorageDto? startStorageDto = null;
                StorageDto? endStorageDto = null;
                if (callMode == "FullOffline")
                {
                    startStorageDto = storage.DeepClone();
                    //放满  起点满容器、终点无容器
                    if (startStorageDto.IsLock)
                    {
                        return BadRequest($"起点工位已被锁定");
                    }
                    if (startStorageDto.State != StorageConst.State.FullContainer)
                    {
                        return BadRequest($"起点工位无满桶");
                    }
                    endStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == area.FullArea).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (endStorageDto == null)
                    {
                        return BadRequest($"满桶下线失败，不存在满桶中转区库位");
                    }
                }
                else if (callMode == "FullOnline")
                {
                    endStorageDto = storage.DeepClone();
                    //放满  起点满容器、终点无容器
                    if (endStorageDto.IsLock)
                    {
                        return BadRequest($"起点工位已被锁定");
                    }
                    if (endStorageDto.State != StorageConst.State.NoneContainer)
                    {
                        return BadRequest($"起点工位存在容器");
                    }
                    //满桶缓存区是否存在指定类型的物料,且库位未锁定
                    var storageFullDtos = await _materialService.GetStoragesByMaterialTypeAsync(area.FullArea, materialType, false);
                    startStorageDto = storageFullDtos.FirstOrDefault();
                    if (startStorageDto == null)
                    {
                        return BadRequest($"满桶上线失败，满桶中转区不存在指定类型满桶库位");
                    }
                    if (startStorageDto.State != StorageConst.State.FullContainer)
                    {
                        return BadRequest($"满桶上线失败，满桶中转区满桶库位状态不为满桶");
                    }
                }
                else if (callMode == "EmptyOffline")
                {
                    //取空   起点空容器、终点无容器
                    startStorageDto = storage.DeepClone();
                    if (startStorageDto.IsLock)
                    {
                        return BadRequest($"空桶下线失败，叫料工位已被锁定");
                    }
                    if (startStorageDto.State == StorageConst.State.NoneContainer)
                    {
                        return BadRequest($"空桶下线失败，叫料工位无料桶");
                    }
                    //1、先判定是否存在空桶中转位,且为未锁定状态
                    endStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == area.EmptyArea && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//放空桶放里层，取空桶取外层(通过标记库位SortNumber)
                    if (endStorageDto == null)
                    {
                        return BadRequest($"空桶下线失败，不存在空桶中转区库位");
                    }
                }
                else if (callMode == "EmptyOnline")
                {
                    //送空   起点空容器、终点无容器
                    endStorageDto = storage.DeepClone();
                    if (endStorageDto.IsLock)
                    {
                        return BadRequest($"空桶上线失败，叫料工位已被锁定");
                    }
                    if (endStorageDto.State != StorageConst.State.NoneContainer)
                    {
                        return BadRequest($"空桶上线失败，叫料工位存在料桶");
                    }
                    //1、先判定是否存在空桶中转位,且为未锁定状态
                    startStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsLock == false && e.AreaCode == area.EmptyArea).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (startStorageDto == null)
                    {
                        return BadRequest($"空桶上线线失败，不存在空桶中转区不存在空桶");
                    }
                }
                else
                {
                    return BadRequest($"模板类型不存在");
                }
                //1、下发点对点搬运任务
                TaskRecordDto taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", startStorageDto, endStorageDto); //取空任务
                taskRecordDto.CallMode = callMode;
                if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
                //2、判定库位容器绑定关系
                var containers = await _storageService.GetContainersAsync(startStorageDto);
                if (containers == null || containers.Count() == 0)
                {
                    return BadRequest($"起点工位[{startStorageDto.Name}]没有容器，任务下发失败");
                }
                var container = containers.First();
                var materials = await _containerService.GetMaterialsAsync(container);
                var material = string.Join(",", materials.Select(e => e.Code));//物料信息
                if (callMode == "EmptyOffline")
                {
                    //空桶下线时，先清掉桶内物料信息.重置桶的状态
                    if (materials != null && materials.Count() > 0)
                        await _containerService.MaterialDeleteAsync(container.Id, materials);
                    container.State = ContainerConst.State.EmptyMaterial;
                    await _containerService.Repository.ExecuteUpdateAsync(e => e.Id == container.Id, s => s.SetProperty(b => b.State, container.State));
                    material = "";
                }
                taskRecordDto.Extend = new TaskRecordExtend
                {
                    ContainerSize = new Service.Models.FlowExtend.ContainerSize
                    {
                        Width = container.Width,
                        Height = container.Height,
                        Length = container.Length
                    },
                    Material = material
                }.ToJson();
                //3、添加work记录
                var result = await _workService.AddWorkAsync(storage.AreaId, callModeName, taskRecordDto, container, material);
                if (result == 0)
                {
                    return BadRequest($"任务提交失败");
                }
                //4、自动发布任务
                var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                try
                {
                    var startNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                    };
                    var containerSize = new Service.Models.FlowExtend.ContainerSize()
                    {
                        Width = container.Width,
                        Length = container.Length,
                        Height = container.Height
                    };
                    var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                        //更新taskid更新work的状态
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

        [HttpPut]
        public async Task<IActionResult> AddWork2([FromBody] TaskRecordDto taskRecordDto)
        {
            CarDto? car = null;
            CarTypeDto? carTypeDto = null;
            if (!string.IsNullOrEmpty(taskRecordDto.CarTypeId))
            {
                carTypeDto = await _carTypeService.FirstOrDefaultAsync(e => e.Id == taskRecordDto.CarTypeId);
                if (carTypeDto == null) return BadRequest($"获取车辆类型失败 [{taskRecordDto.CarTypeId}]");
            }
            var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
            if (taskRecordDto.SrcStorageId == taskRecordDto.DestStorageId)
                return BadRequest($"呼叫库位和选择的库位不能相同");
            var startStorageDto = await _storageService.Set().Where(e => e.Id == taskRecordDto.SrcStorageId).FirstOrDefaultAsync();
            if (startStorageDto == null) return BadRequest($"获取起点库位失败 [{taskRecordDto.SrcStorageId}]");
            var endStorageDto = await _storageService.Set().Where(e => e.Id == taskRecordDto.DestStorageId).FirstOrDefaultAsync();
            if (endStorageDto == null) return BadRequest($"获取终点库位失败 [{taskRecordDto.DestStorageId}]");
            var callModeName = string.Empty;
            if (taskRecordDto.CallMode == "FullOffline" || taskRecordDto.CallMode == "FullOnline")
            {
                //放满/送满  起点满容器、终点无容器
                if (startStorageDto.IsLock)
                {
                    return BadRequest($"起点工位已被锁定");
                }
                if (startStorageDto.State != StorageConst.State.FullContainer)
                {
                    return BadRequest($"起点工位无满桶");
                }
                if (endStorageDto.IsLock)
                {
                    return BadRequest($"终点工位已被锁定");
                }
                if (endStorageDto.State != StorageConst.State.NoneContainer)
                {
                    return BadRequest($"终点工位存在容器");
                }
                callModeName = $"{startStorageDto.Name}";
            }
            else if (taskRecordDto.CallMode == "EmptyOnline" || taskRecordDto.CallMode == "EmptyOffline")
            {
                //取空/送空   起点空容器、终点无容器
                if (startStorageDto.IsLock)
                {
                    return BadRequest($"起点工位已被锁定");
                }
                if (startStorageDto.State != StorageConst.State.EmptyContainer)
                {
                    return BadRequest($"起点工位无空容器");
                }
                if (endStorageDto.IsLock)
                {
                    return BadRequest($"终点工位已被锁定");
                }
                if (endStorageDto.State != StorageConst.State.NoneContainer)
                {
                    return BadRequest($"终点工位存在容器");
                }
                callModeName = $"{endStorageDto.Name}";
            }
            else
            {
                return BadRequest($"模板类型不存在");
            }

            var dictItem = await _dictItemService.FirstOrDefaultAsync(e => e.Code == taskRecordDto.CallMode && e.IsEnable && e.Dict.Code == "CommonCallMode");
            if (dictItem == null)
            {
                return BadRequest($"模板类型不存在 [{taskRecordDto.CallMode}]");
            }
            callModeName += $"-{dictItem.Name}";//拼接工位-模板名称
            taskRecordDto.TaskTemplateId = "Double";//使用双点模板
            taskRecordDto.Type = "Template";//默认模板类型
            taskRecordDto.SrcNodeId = startStorageDto.NodeId;
            taskRecordDto.SrcNodeCode = startStorageDto.NodeCode;
            taskRecordDto.SrcAreaId = startStorageDto.AreaId;
            taskRecordDto.DestNodeId = endStorageDto.NodeId;
            taskRecordDto.DestNodeCode = endStorageDto.NodeCode;
            taskRecordDto.DestAreaId = endStorageDto.AreaId;
            taskRecordDto.Name = $"车辆 [{car?.Code}] 从起点站点 [{taskRecordDto.SrcNodeCode}] 到终点站点 [{taskRecordDto.DestNodeCode}]";
            taskRecordDto.Description = $"{startStorageDto.Code} => {endStorageDto.Code}";
            taskRecordDto.Id = GuidHelper.GetGuidSequential().ToString();
            taskRecordDto.State = TaskRecordConst.State.Created;
            taskRecordDto.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //1、判定库位容器绑定关系
            var containers = await _storageService.GetContainersAsync(startStorageDto);
            if (containers == null || containers.Count() == 0)
            {
                return BadRequest($"起点工位[{startStorageDto.Name}]没有容器，任务下发失败");
            }
            var containers2 = await _storageService.GetContainersAsync(endStorageDto);
            if (containers2.Count() > 0)
            {
                return BadRequest($"终点工位[{endStorageDto.Name}]存在容器，任务下发失败");
            }
            var container = containers.First();
            var materials = await _containerService.GetMaterialsAsync(container);
            var material = string.Join(",", materials.Select(e => e.Code));//物料信息
            taskRecordDto.Extend = new TaskRecordExtend
            {
                ContainerSize = new Service.Models.FlowExtend.ContainerSize
                {
                    Width = container.Width,
                    Height = container.Height,
                    Length = container.Length
                },
                Material = material
            }.ToJson();
            //2、添加work记录
            var result = await _workService.AddWorkAsync(endStorageDto.AreaId, callModeName, taskRecordDto, container, material);
            if (result == 0)
            {
                return BadRequest($"任务提交失败");
            }
            //3、自动发布任务
            var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
            try
            {
                var startNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                };
                var endNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                };
                var containerSize = new Service.Models.FlowExtend.ContainerSize()
                {
                    Width = container.Width,
                    Length = container.Length,
                    Height = container.Height
                };
                var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, car, carTypeCode);
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                    //更新taskid更新work的状态
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

            return Ok();
        }

        //四院平板呼叫
        [HttpPut]
        public async Task<IActionResult> AddWork3([FromQuery] string storageId, [FromQuery] string type, [FromQuery] string callMode = "EmptyOnline", [FromQuery] string carTypeId = "")
        {
            //判断车型是否存在
            CarTypeDto? carTypeDto = null;
            if (!string.IsNullOrEmpty(carTypeId))
            {
                carTypeDto = await _carTypeService.FirstOrDefaultAsync(e => e.Id == carTypeId);
                if (carTypeDto == null) return BadRequest($"获取车辆类型失败 [{carTypeId}]");
            }
            var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
            //判断叫料库位是否存在
            var storage = await _storageService.FirstOrDefaultAsync(e => e.Id == storageId && e.IsEnable && !string.IsNullOrEmpty(e.NodeCode));
            if (storage == null)
            {
                return BadRequest($"库位不存在,库位编号 [{storageId}]");
            }
            var dictItem = await _dictItemService.FirstOrDefaultAsync(e => e.Code == callMode && e.IsEnable && e.Dict.Code == "CommonCallMode");
            if (dictItem == null)
            {
                return BadRequest($"模板类型不存在 [{callMode}]");
            }
            var callModeName = $"{storage.Name}-{dictItem.Name}";//拼接工位-模板名称
            //var callModeName = $"{storage.Name}-送空";
            //单步动作
            TaskRecordDto taskRecordDto = new TaskRecordDto()
            {
                TaskTemplateId = "",
                SrcNodeId = "",
                DestNodeId = ""
            };
            StorageDto? startStorageDto;
            StorageDto? endStorageDto;
            if (type == "Call")
            {
                if (storage.AreaCode == "FYSJ")
                {
                    //废药收集区呼叫，起点是当前点，终点是废药暂存展示区(FYZC)
                    startStorageDto = storage.DeepClone();
                    if (startStorageDto.IsLock)
                    {
                        return BadRequest($"起点工位已被锁定");
                    }
                    if (startStorageDto.State != StorageConst.State.EmptyContainer)
                    {
                        return BadRequest($"起点工位无空桶");
                    }
                    endStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsEnable && e.IsLock == false && e.AreaCode == "FYZC").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (endStorageDto == null)
                    {
                        return BadRequest($"废药收集区呼叫失败，不存在废药暂存库位");
                    }
                }
                else
                {
                    //废药摆放展示区呼叫，起点是废药暂存展示区(FYZC)，终点是当前站点
                    startStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsEnable && e.IsLock == false && e.AreaCode == "FYZC").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    if (startStorageDto == null)
                    {
                        return BadRequest($"废药摆放展示区呼叫失败，废药暂存库位不存在空桶");
                    }
                    endStorageDto = storage.DeepClone();
                }
            }
            else if (type == "Release")
            {
                // 废药摆放展示区呼叫 放行， 起点当前站点，终点 废药收集区(废药收集展示区)
                startStorageDto = storage.DeepClone();
                //if (startStorageDto.IsLock)
                //{
                //    return BadRequest($"起点工位已被锁定");
                //}
                if (startStorageDto.State != StorageConst.State.EmptyContainer)
                {
                    return BadRequest($"起点工位无空桶");
                }
                endStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.IsEnable && e.AreaCode == "FYSJ").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                if (endStorageDto == null)
                {
                    return BadRequest($"废药摆放展示区放行失败，不存收集区空库位");
                }
            }
            else
            {
                return BadRequest($"呼叫类型不存在");
            }
            //1、下发点对点搬运任务
            taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", startStorageDto, endStorageDto); //取空任务
            taskRecordDto.CallMode = callMode;
            if (carTypeDto != null) taskRecordDto.CarTypeId = carTypeDto.Id;
            //2、判定库位容器绑定关系
            var containers = await _storageService.GetContainersAsync(startStorageDto);
            if (containers == null || containers.Count() == 0)
            {
                return BadRequest($"起点工位[{startStorageDto.Name}]没有容器，任务下发失败");
            }
            var container = containers.First();
            //var materials = await _containerService.GetMaterialsAsync(container);
            //var material = string.Join(",", materials.Select(e => e.Code));//物料信息
            var material = "";
            taskRecordDto.Extend = new TaskRecordExtend
            {
                ContainerSize = new Service.Models.FlowExtend.ContainerSize
                {
                    Width = container.Width,
                    Height = container.Height,
                    Length = container.Length
                },
                Material = material
            }.ToJson();
            //3、添加work记录
            var result = await _workService.AddWorkAsync(storage.AreaId, callModeName, taskRecordDto, container, material);
            if (result == 0)
            {
                return BadRequest($"任务提交失败");
            }
            //4、自动发布任务
            var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
            try
            {
                var startNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                };
                var endNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                };
                var containerSize = new Service.Models.FlowExtend.ContainerSize()
                {
                    Width = container.Width,
                    Length = container.Length,
                    Height = container.Height
                };
                var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null, carTypeCode);
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                    //更新taskid更新work的状态
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
            return Ok();
        }


    }
}
