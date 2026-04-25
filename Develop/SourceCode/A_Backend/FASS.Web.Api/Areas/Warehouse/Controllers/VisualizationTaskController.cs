using Common.Frame.Services.Frame.Interfaces;
using Common.Service.Pager.Models;
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
using FASS.Service.Extends.Warehouse;
using FASS.Service.Services.BaseExtend.Interfaces;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc.Extensions;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Warehouse.Controllers
{
    [Route("api/v1/Warehouse/[controller]/[action]")]
    [Tags("仓储管理-库位任务可视化")]
    public class VisualizationTaskController : BaseController
    {
        private readonly ILogger<VisualizationTaskController> _logger;
        private readonly IAreaService _areaService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly ICarService _carService;
        private readonly ICarTypeService _carTypeService;
        private readonly INodePositionService _nodePositionService;
        private readonly IWorkService _workService;
        private readonly IDictItemService _dictItemService;
        private readonly AppSettings _appSettings;

        public VisualizationTaskController(
            ILogger<VisualizationTaskController> logger,
            IAreaService areaService,
            ITaskRecordService taskRecordService,
            IStorageService storageService,
            IContainerService containerService,
            IMaterialService materialService,
            ICarService carService,
            ICarTypeService carTypeService,
            INodePositionService nodePositionService,
            IWorkService workService,
            IDictItemService dictValueService,
            AppSettings appSettings)
        {
            _logger = logger;
            _areaService = areaService;
            _taskRecordService = taskRecordService;
            _storageService = storageService;
            _containerService = containerService;
            _materialService = materialService;
            _carService = carService;
            _carTypeService = carTypeService;
            _nodePositionService = nodePositionService;
            _workService = workService;
            _dictItemService = dictValueService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> StorageGetPage([FromQuery] string? keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _areaService.StorageGetPageAsync(keyValue, param);
            //获取库位容器对应关系
            var storageContainerDtos = await _storageService.GetStorageContainerListAsync("");
            //获取容器信息
            var containerDtos = await _containerService.Set().Where(e => e.IsEnable).ToListAsync();
            //获取容器物料对应关系
            var containerMaterialDtos = await _containerService.GetContainerMaterialListAsync("");
            //获取物料信息
            var materialDtos = await _materialService.Set().Where(e => e.IsEnable).ToListAsync();
            //获取站点坐标信息
            var nodePositionDtos = await _nodePositionService.Set().Where(e => page.Data.Select(p => p.NodeId).Contains(e.NodeId)).ToListAsync();
            for (var i = 0; i < page.Data.Count; i++)
            {
                //获取储位的容器
                var containerIds = storageContainerDtos.Where(e => e.StorageId == page.Data[i].Id).Select(e => e.ContainerId).ToList();
                page.Data[i].Containers = containerDtos.Where(e => containerIds.Contains(e.Id)).ToList();
                //获取容器的物料
                if (containerIds != null && containerIds.Count > 0)
                {
                    var materialIds = containerMaterialDtos.Where(e => containerIds.Contains(e.ContainerId)).Select(e => e.MaterialId).ToList();
                    page.Data[i].Materials = materialDtos.Where(e => materialIds.Contains(e.Id)).ToList();
                }
                var nodePositionDto = nodePositionDtos.Where(e => e.NodeId == page.Data[i].NodeId).FirstOrDefault();
                if (nodePositionDto != null)
                {
                    page.Data[i].Coordinate = [nodePositionDto.X, nodePositionDto.Y];
                    var extend = page.Data[i]?.Extend?.JsonTo<StorageExtend>();
                    if (extend != null)
                    {
                        page.Data[i].OffsetCoordinate = extend.OffsetCoordinate;
                        page.Data[i].TextCoordinate = extend.TextCoordinate;
                        page.Data[i].Coefficient = extend?.Coefficient ?? 1;
                    }
                }
            }
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddWork([FromBody] TaskRecordDto taskRecordDto)
        {
            CarDto? car = null;
            CarTypeDto? carTypeDto = null;
            if (!string.IsNullOrEmpty(taskRecordDto.CarId))
            {
                car = await _carService.FirstOrDefaultAsync(e => e.Id == taskRecordDto.CarId);
                if (car == null) return BadRequest($"获取车辆失败 [{taskRecordDto.CarId}]");
            }
            if (!string.IsNullOrEmpty(taskRecordDto.CarTypeId))
            {
                carTypeDto = await _carTypeService.FirstOrDefaultAsync(e => e.Id == taskRecordDto.CarTypeId);
                if (carTypeDto == null) return BadRequest($"获取车辆类型失败 [{taskRecordDto.CarTypeId}]");
            }
            var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
            if (taskRecordDto.CallMode == "EmptyFullExchange" || taskRecordDto.CallMode == "FullEmptyExchange")
            {
                if (taskRecordDto.SrcStorageId == taskRecordDto.DestStorageId
                    || taskRecordDto.SrcStorageId == taskRecordDto.CallStorageId
                    || taskRecordDto.DestStorageId == taskRecordDto.CallStorageId)
                    return BadRequest($"{(taskRecordDto.CallMode == "EmptyFullExchange" ? "放空库位、取满库位" : "放满库位、取空库位")}、呼叫库位都不能相同");
                //判断叫料库位是否存在
                var storage = await _storageService.Set().Where(e => e.Id == taskRecordDto.CallStorageId && e.IsEnable && !e.IsDelete).FirstOrDefaultAsync();
                if (storage == null)
                {
                    return BadRequest($"呼叫库位不存在，库位编号 [{taskRecordDto.CallStorageId}]");
                }
                if (storage.IsLock)
                {
                    return BadRequest($"叫料失败，叫料工位已被锁定");
                }
                var startStorageDto = await _storageService.Set().Where(e => e.Id == taskRecordDto.SrcStorageId && e.IsEnable && !e.IsDelete).FirstOrDefaultAsync();
                var endStorageDto = await _storageService.Set().Where(e => e.Id == taskRecordDto.DestStorageId && e.IsEnable && !e.IsDelete).FirstOrDefaultAsync();
                if (taskRecordDto.CallMode == "FullEmptyExchange")
                {
                    //满空交换
                    //1、叫料库位是否存在且为满料
                    if (storage.State != StorageConst.State.FullContainer)
                    {
                        return BadRequest($"取满放空失败，叫料工位无满容器");
                    }
                    //2、叫料库位是否满料
                    var fullContainers = await _storageService.GetContainersAsync(storage);
                    if (fullContainers == null || fullContainers.Count() == 0)
                    {
                        return BadRequest($"叫料库位[{storage.Name}]没有容器，任务下发失败");
                    }
                    var container = fullContainers.First();//获取满容器
                    if (container.State != ContainerConst.State.FullMaterial)
                    {
                        return BadRequest($"叫料库位[{storage.Name}]容器不为满料，任务下发失败");
                    }
                    var materialDtos = await _containerService.GetMaterialsAsync(container);
                    //3、判定放满库位是否未锁定状态
                    if (startStorageDto == null)
                    {
                        return BadRequest($"放满库位不存在，库位编号 [{taskRecordDto.SrcStorageId}]");
                    }
                    if (startStorageDto.IsLock)
                    {
                        return BadRequest($"叫料失败，放满库位{taskRecordDto.SrcStorageId}已被锁定");
                    }
                    if (startStorageDto.State != StorageConst.State.NoneContainer)
                    {
                        return BadRequest($"取满放空失败，放满库位{taskRecordDto.SrcStorageId}已有容器");
                    }
                    //4、判断取空库位是否有空桶且为未锁定状态
                    if (endStorageDto == null)
                    {
                        return BadRequest($"取空库位不存在，库位编号 [{taskRecordDto.DestStorageId}]");
                    }
                    if (endStorageDto.IsLock)
                    {
                        return BadRequest($"叫料失败，取空库位{taskRecordDto.DestStorageId}已被锁定");
                    }
                    if (endStorageDto.State != StorageConst.State.EmptyContainer)
                    {
                        return BadRequest($"取满放空失败，取空库位{taskRecordDto.DestStorageId}无空容器");
                    }
                    var emptyContainers = await _storageService.GetContainersAsync(endStorageDto);
                    if (emptyContainers == null || emptyContainers.Count() == 0)
                    {
                        return BadRequest($"取空库位[{endStorageDto.Name}]没有容器，任务下发失败");
                    }
                    var emptyContainer = emptyContainers.First();
                    if (emptyContainer.State != ContainerConst.State.EmptyMaterial)
                    {
                        return BadRequest($"取空库位[{endStorageDto.Name}]容器不为空容器，任务下发失败");
                    }
                    //5、变更库位锁状态
                    storage.IsLock = true;
                    startStorageDto.IsLock = true;
                    endStorageDto.IsLock = true;
                    //await _storageService.UpdateAsync(new List<StorageDto> { storage, startStorageDto, endStorageDto });
                    //6、下发点对点搬运任务
                    var taskRecordNewDto = CarTaskExtension.CreateTaskRecord("Double", storage, startStorageDto); //取满任务
                    taskRecordNewDto.Code = $"{taskRecordNewDto.Code}-1";
                    taskRecordNewDto.CarId = car?.Id;//添加车辆任务
                    taskRecordNewDto.CarTypeId = carTypeDto?.Id;//添加车型
                    taskRecordNewDto.Priority = 1;
                    var followupTaskDto = CarTaskExtension.CreateTaskRecord("Double", endStorageDto, storage);//送空任务
                    followupTaskDto.Code = $"{taskRecordNewDto.Code.Replace("-1", "-2")}";
                    followupTaskDto.CarTypeId = carTypeDto?.Id;//添加车型
                    //7、将后置任务记录id存入前置任务Extend字段
                    var materials = string.Join(",", materialDtos.Select(e => e.Code));
                    taskRecordNewDto.Extend = new TaskRecordExtend
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
                    //8、添加work记录
                    //1)取满桶
                    var callModeName = "满桶下线/空桶上线";
                    var result = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(下满桶)", taskRecordNewDto, container, materials, "FullEmptyExchange");
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
                    var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordNewDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
                    try
                    {
                        var containerSize = new Service.Models.FlowExtend.ContainerSize()
                        {
                            Width = container.Width,
                            Length = container.Length,
                            Height = container.Height
                        };
                        var startNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(taskRecordNewDto.SrcNodeCode) };
                        var endNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(taskRecordNewDto.DestNodeCode) };
                        var req = CarTaskExtension.ToCarTask(taskRecordNewDto, startNode, endNode, containerSize, car, carTypeCode);
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
                }
                else
                {
                    //空满交换
                    //1、叫料库位是否存在且为空容器
                    if (storage.State != StorageConst.State.EmptyContainer)
                    {
                        return BadRequest($"取空放满失败，呼叫库位无空容器");
                    }
                    //2、叫料库位容器是否是空容器
                    var emptyContainers = await _storageService.GetContainersAsync(storage);
                    if (emptyContainers == null || emptyContainers.Count() == 0)
                    {
                        return BadRequest($"取空库位[{storage.Name}]没有容器，任务下发失败");
                    }
                    var emptyContainer = emptyContainers.First();
                    if (emptyContainer.State != ContainerConst.State.EmptyMaterial)
                    {
                        return BadRequest($"取空库位[{storage.Name}]容器不为空容器，任务下发失败");
                    }
                    //3、判定放空库位是否未锁定状态，且库位无容器
                    if (startStorageDto == null)
                    {
                        return BadRequest($"放空库位不存在，库位编号 [{taskRecordDto.SrcStorageId}]");
                    }
                    if (startStorageDto.IsLock)
                    {
                        return BadRequest($"叫料失败，放空库位{taskRecordDto.SrcStorageId}已被锁定");
                    }
                    if (startStorageDto.State != StorageConst.State.NoneContainer)
                    {
                        return BadRequest($"取空放满失败，放空库位{taskRecordDto.SrcStorageId}已有容器");
                    }
                    //4、判断取满库位是否有满桶且为未锁定状态
                    if (endStorageDto == null)
                    {
                        return BadRequest($"取满库位不存在，库位编号 [{taskRecordDto.DestStorageId}]");
                    }
                    if (endStorageDto.IsLock)
                    {
                        return BadRequest($"叫料失败，取满库位{taskRecordDto.DestStorageId}已被锁定");
                    }
                    if (endStorageDto.State != StorageConst.State.FullContainer)
                    {
                        return BadRequest($"取空放满失败，取满库位{taskRecordDto.DestStorageId}无满容器");
                    }
                    var fullContainers = await _storageService.GetContainersAsync(endStorageDto);
                    if (fullContainers == null || fullContainers.Count() == 0)
                    {
                        return BadRequest($"取满库位[{endStorageDto.Name}]没有容器，任务下发失败");
                    }
                    var fullContainer = fullContainers.First();
                    if (fullContainer.State != ContainerConst.State.FullMaterial)
                    {
                        return BadRequest($"取满库位[{endStorageDto.Name}]容器不为满容器，任务下发失败");
                    }
                    var materialDtos = await _containerService.GetMaterialsAsync(fullContainer);
                    //5、变更库位锁状态
                    storage.IsLock = true;
                    startStorageDto.IsLock = true;
                    endStorageDto.IsLock = true;
                    //await _storageService.UpdateAsync(new List<StorageDto> { storage, startStorageDto, endStorageDto });
                    //6、下发点对点搬运任务
                    var taskRecordNewDto = CarTaskExtension.CreateTaskRecord("Double", storage, startStorageDto); //取空任务
                    taskRecordNewDto.Code = $"{taskRecordNewDto.Code}-1";
                    taskRecordNewDto.CarId = car?.Id;//添加车辆任务
                    taskRecordNewDto.CarTypeId = carTypeDto?.Id;//添加车型
                    taskRecordNewDto.Priority = 1;
                    var followupTaskDto = CarTaskExtension.CreateTaskRecord("Double", endStorageDto, storage);//送满任务
                    followupTaskDto.Code = $"{taskRecordNewDto.Code.Replace("-1", "-2")}";
                    followupTaskDto.CarTypeId = carTypeDto?.Id;//添加车型
                    //7、将后置任务记录id存入前置任务Extend字段
                    taskRecordNewDto.Extend = new TaskRecordExtend
                    {
                        FollowUpTaskId = followupTaskDto.Id,
                        ContainerSize = new Service.Models.FlowExtend.ContainerSize
                        {
                            Width = emptyContainer.Width,
                            Height = emptyContainer.Height,
                            Length = emptyContainer.Length
                        }
                    }.ToJson();
                    //8、添加work记录
                    //1)取空桶
                    var callModeName = "空桶下线/满桶上线";
                    var result = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(下空桶)", taskRecordNewDto, emptyContainer, "", "EmptyFullExchange");
                    if (result == 0)
                    {
                        return BadRequest($"空满桶交换任务-取空桶提交失败");
                    }
                    //2)送满桶
                    var materials = string.Join(",", materialDtos.Select(e => e.Code));
                    followupTaskDto.Extend = new TaskRecordExtend
                    {
                        ContainerSize = new Service.Models.FlowExtend.ContainerSize
                        {
                            Width = fullContainer.Width,
                            Height = fullContainer.Height,
                            Length = fullContainer.Length
                        },
                        Material = materials
                    }.ToJson();
                    var result2 = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(上空桶)", followupTaskDto, fullContainer, materials);
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
                        var req = CarTaskExtension.ToCarTask(taskRecordNewDto, startNode, endNode, containerSize, car, carTypeCode);
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
                }
            }
            else
            {
                if (taskRecordDto.SrcStorageId == taskRecordDto.DestStorageId)
                    return BadRequest($"起点库位和终点库位不能相同");
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
                        Code = taskRecordDto.SrcNodeCode
                    };
                    var endNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = taskRecordDto.DestNodeCode
                    };
                    var containerSize = new Service.Models.FlowExtend.ContainerSize()
                    {
                        Width = container.Width,
                        Length = container.Length,
                        Height = container.Height
                    };
                    //var carTypeCode = carTypeDto == null ? "Car" : carTypeDto.Code;
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
            }

            return Ok();
        }


        /// <summary>
        /// 点击设置显示绑定的库位信息
        /// </summary>
        /// <param name="keyValue">库位信息id</param>
        /// <param name="pageParam">分页查询的参数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ContainerGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _storageService.ContainerGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
        /// <summary>
        /// 获取指定容器绑定物料的信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MaterialGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            //获取库位容器对应关系
            var storageContainerDtos = await _storageService.GetStorageContainerListAsync(keyValue);
            if (storageContainerDtos.Count() == 0)
            {
                return BadRequest("目前库位没有绑定容器无法绑定物料，请先绑定容器");
            }
            var storageContainer = Guard.NotNull(storageContainerDtos.ToList().FirstOrDefault());
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _containerService.MaterialGetPageAsync(storageContainer.ContainerId, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
        /// <summary>
        /// 获取所有可用没锁定的物料信息
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetListToSelectMaterial([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _materialService.SelectGetPageIsNoLockAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        /// <summary>
        /// 获取可用的容器(IsEnable = true)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetListToSelectContainer([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var data = await _containerService.SelectGetPageAsync(param);
            return Ok(data);
        }

        /// <summary>
        /// 库位添加容器
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="containerDtos"></param>
        /// <returns></returns>

        [HttpPut]

        public async Task<IActionResult> StorageAddContainer([FromQuery] string keyValue, [FromBody] List<ContainerDto> containerDtos)
        {
            if (containerDtos.Count() > 1)
            {
                return BadRequest("一个库位只容许添加一个容器");
            }
            await _storageService.ContainerAddAsync(keyValue, containerDtos);
            return Ok();
        }

        /// <summary>
        /// 容器绑定物料
        /// </summary>
        /// <param name="keyValue">容器ID</param>
        /// <param name="materialDtos">物料实体类</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ContainerAddMaterial([FromQuery] string keyValue, [FromBody] List<MaterialDto> materialDtos)
        {

            var result = await _containerService.MaterialAddAsync(keyValue, materialDtos);
            if (result == 0)
            {
                return BadRequest("容器已绑定物料，请解绑后再绑定");
            }
            return Ok();
        }
        /// <summary>
        /// 库位删除容器
        /// </summary>
        /// <param name="keyValue">库位ID</param>
        /// <param name="containerDtos"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> ContainerDelete([FromQuery] string keyValue, [FromBody] List<ContainerDto> containerDtos)
        {
            if (keyValue == null || keyValue == "")
            {
                return BadRequest("未指定需要删除物料的库位");
            }
            await _storageService.ContainerDeleteAsync(keyValue, containerDtos);
            return Ok();
        }

        /// <summary>
        /// 容器删除物料
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="materialDtos"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> MaterialDelete([FromQuery] string keyValue, [FromBody] List<MaterialDto> materialDtos)
        {
            if (keyValue == null || keyValue == "")
            {
                return BadRequest("未指定需要删除物料的容器");
            }
            await _containerService.MaterialDeleteAsync(keyValue, materialDtos);
            return Ok();
        }


        /// <summary>
        /// 库位信息的修改和增加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="storageDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> StorageAddOrUpdate([FromQuery] string keyValue, [FromBody] StorageDto storageDto)
        {
            if (keyValue == null || keyValue == "")
            {
                return BadRequest("未指定需要需要修改的库位");
            }
            var result = await _storageService.AddOrUpdateAsync(keyValue, storageDto);
            if (result == 0)
            {
                return BadRequest("修改库位状态违规，请移除容器或者确认容器状态后修改");
            }
            return Ok();
        }

        /// <summary>
        /// 容器状态的增加和修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="containerDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ContainerAddOrUpdate([FromQuery] string keyValue, [FromBody] ContainerDto containerDto)
        {
            if (keyValue == null || keyValue == "")
            {
                return BadRequest("未指定需要需要修改的容器");
            }
            var result = await _containerService.AddOrUpdateAsync(keyValue, containerDto);
            if (result == 0)
            {
                return BadRequest("修改容器状态前先解除绑定的物料");
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Save([FromBody] List<StorageDto> storageDtos)
        {
            var dtos = await _storageService.Set().AsNoTracking().Where(e => e.IsEnable).ToListAsync();
            dtos.ForEach(d =>
            {
                var dto = storageDtos.Where(e => e.Id == d.Id).FirstOrDefault();
                if (dto != null)
                {
                    var extend = new StorageExtend
                    {
                        OffsetCoordinate = dto.OffsetCoordinate,
                        TextCoordinate = dto.TextCoordinate,
                        Coefficient = dto.Coefficient,
                        Sketchpad = dto.Sketchpad
                    };
                    d.Extend = extend.ToJson();
                }
                else
                {
                    if (d.Extend is not null)
                    {
                        var extend = d.Extend.JsonTo<StorageExtend>();
                        if (extend is not null)
                        {
                            extend.TextCoordinate = storageDtos[0].TextCoordinate;
                            extend.Coefficient = storageDtos[0].Coefficient;
                            extend.Sketchpad = storageDtos[0].Sketchpad;
                            d.Extend = extend.ToJson();
                        }
                    }
                }
            });
            await _storageService.UpdateAsync(dtos);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ResetAll()
        {
            var dtos = await _storageService.Set().Where(e => e.IsEnable).ToListAsync();
            dtos.ForEach(d =>
            {
                var extend = new StorageExtend
                {
                    OffsetCoordinate = [0, 0],
                    TextCoordinate = [0, 0],
                    Coefficient = 1
                };
                d.Extend = extend.ToJson();
            });
            await _storageService.UpdateAsync(dtos);
            return Ok();
        }

    }
}
