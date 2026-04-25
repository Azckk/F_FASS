using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using DotNetCore.CAP;
using FASS.Boot.Services;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Models.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Scheduler.Attributes;
using FASS.Scheduler.Controllers.Base;
using FASS.Scheduler.Controllers.Extensions;
using FASS.Scheduler.Controllers.Models.Request;
using FASS.Scheduler.Models;
using FASS.Scheduler.Utility;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;
using FASS.Service.Extends.Flow;
using FASS.Service.Models.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FASS.Scheduler.Controllers
{

    [AllowAnonymous]
    [TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
    [TypeFilter(typeof(ActionLogIgonreAttribute))]
    [Tags("mdcs接口")]
    public class MdcsController : BaseController
    {
        private readonly ILogger<MdcsController> _logger;
        private readonly IBootService _bootService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly IWorkService _workService;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly ICarService _carService;
        private readonly ICapPublisher _capPublisher;
        public AppSettings _appSettings { get; private set; }

        public MdcsController(
            ILogger<MdcsController> logger,
            IBootService bootService,
            ITaskInstanceService taskInstanceService,
            IWorkService workService,
            IStorageService storageService,
            IContainerService containerService,
            ICarService carService,
            ICapPublisher capPublisher,
            AppSettings appSettings,
            ITaskRecordService taskRecordService)
        {
            _logger = logger;
            _bootService = bootService;
            _taskInstanceService = taskInstanceService;
            _carService = carService;
            _capPublisher = capPublisher;
            _taskRecordService = taskRecordService;
            _workService = workService;
            _storageService = storageService;
            _containerService = containerService;
            _appSettings = appSettings;
        }

        [Tags("状态回调")]
        [HttpPost]
        public IActionResult State(Models.Request.TaskState request)
        {
            var car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
            if (car == null)
            {
                return BadRequest($"获取车辆失败 [{request.CarCode}]");
            }
            var taskInstanceDto = _taskInstanceService.FirstOrDefault(e => e.Id == request.MissionId);
            if (taskInstanceDto == null)
            {
                return BadRequest($"获取任务失败 [{request.MissionId}]");
            }
            var taskRecordDto = _taskRecordService.FirstOrDefault(e => e.Id == request.MissionId);
            if (taskRecordDto == null)
            {
                //只有实例实例无任务记录时,直接返回调用成功
                return Ok(new
                {
                    missionId = request.MissionId,
                    carCode = car.Code,
                    state = request.State,
                    triggerTime = request.TriggerTime
                });
            }
            var workDto = _workService.FirstOrDefault(e => e.TaskId == request.MissionId);
            if (string.IsNullOrEmpty(taskRecordDto.CarId))
            {
                taskInstanceDto.CarId = car.Id;
                taskInstanceDto.CarTypeId = car.CarTypeId;
                taskInstanceDto.Name = taskInstanceDto.Name?.Replace("[]", $"[{car.Code}]");
                taskRecordDto.CarId = car.Id;
                taskRecordDto.CarTypeId = car.CarTypeId;
                taskRecordDto.Name = taskRecordDto.Name?.Replace("[]", $"[{car.Code}]");
            }
            switch (request.State)
            {
                case TaskState.MissionStateEnum.Started:
                    if (taskRecordDto.State != TaskRecordConst.State.Canceled && taskRecordDto.State != TaskRecordConst.State.Faulted)
                    {
                        taskRecordDto.State = TaskRecordConst.State.Fetching;
                        taskRecordDto.StartTime = request.TriggerTime;
                        taskInstanceDto.State = TaskInstanceConst.State.Released;
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Running;
                        }
                    }
                    break;
                case TaskState.MissionStateEnum.Fetched:
                    if (taskRecordDto.State != TaskRecordConst.State.Canceled && taskRecordDto.State != TaskRecordConst.State.Faulted)
                    {
                        taskRecordDto.State = TaskRecordConst.State.Putting;
                        if (workDto != null)
                        {
                            /*
                             *  取货完成
                             *  1、释放库位锁,修改库位状态
                             *  2、解除库位与容器绑定关系[添加历史绑定记录]
                             */
                            var storageSrcDto = _storageService.Set().AsNoTracking().FirstOrDefault(e => e.NodeCode == taskRecordDto.SrcNodeCode);
                            if (storageSrcDto is not null)
                            {
                                if (workDto.Extend is null || (workDto.Extend is not null && !workDto.Extend.Contains("EmptyFullExchange") && !workDto.Extend.Contains("FullEmptyExchange")))
                                {
                                    //非组合动作第一个动作时释放工位锁
                                    storageSrcDto.IsLock = false;
                                }
                                storageSrcDto.State = StorageConst.State.NoneContainer;
                                _storageService.Repository.ExecuteUpdate(e => e.Id == storageSrcDto.Id, s => s.SetProperty(b => b.State, StorageConst.State.NoneContainer).SetProperty(b => b.IsLock, storageSrcDto.IsLock));
                                _storageService.ContainerDelete(storageSrcDto.Id, new List<string> { workDto.ContainerId });
                            }
                        }
                    }
                    break;
                case TaskState.MissionStateEnum.Put:
                    //taskRecordDto.State = TaskRecordConst.State.Completed;
                    /*
                     *  放货完成
                     *  1、释放库位锁
                     *  2、添加库位与容器绑定关系[添加历史绑定记录]
                     */
                    var storageDestDto = _storageService.Set().AsNoTracking().FirstOrDefault(e => e.NodeCode == taskRecordDto.DestNodeCode);
                    if (storageDestDto != null && workDto != null)
                    {
                        storageDestDto.IsLock = false;
                        //判定是空桶还是满桶[容器是否有物料]
                        var materials = _containerService.GetMaterials(workDto.ContainerId);
                        storageDestDto.State = (materials != null && materials.Count() > 0) ? StorageConst.State.FullContainer : StorageConst.State.EmptyContainer;
                        //_storageService.Update(storageDestDto);
                        _storageService.Repository.ExecuteUpdate(e => e.Id == storageDestDto.Id, s => s.SetProperty(b => b.State, storageDestDto.State).SetProperty(b => b.IsLock, storageDestDto.IsLock));
                        _storageService.ContainerAdd(storageDestDto.Id, new List<string> { workDto.ContainerId });
                    }
                    break;
                case TaskState.MissionStateEnum.Finished:
                    if (taskRecordDto.State != TaskRecordConst.State.Canceled && taskRecordDto.State != TaskRecordConst.State.Faulted)
                    {
                        taskRecordDto.State = TaskRecordConst.State.Completed;
                        taskRecordDto.EndTime = request.TriggerTime;
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Completed;
                        }
                        var taskInstance = car.TaskInstances.Where(e => e.Id == taskInstanceDto.Id).FirstOrDefault();
                        if (taskInstance != null && taskInstance.State != TaskInstanceConst.State.Completing && taskInstance.State != TaskInstanceConst.State.Completed)
                        {
                            //更新之前的任务实例
                            var leftTaskDto = car.TaskInstances.Where(e => e.State == TaskInstanceConst.State.Running && e.Id != taskInstanceDto.Id && e.CreateAt < taskInstanceDto.CreateAt).ToList().FirstOrDefault();
                            if (leftTaskDto is not null)
                            {
                                leftTaskDto.State = TaskInstanceConst.State.Completing;
                                leftTaskDto.TaskInstanceProcesses.Where(e => e.State != TaskInstanceProcessConst.State.Completing && e.State != TaskInstanceProcessConst.State.Completed).ForEach(dr =>
                                {
                                    dr.State = TaskInstanceProcessConst.State.Completed;
                                });
                                _taskInstanceService.Repository.ExecuteUpdate(e => e.Id == leftTaskDto.Id, s => s.SetProperty(b => b.State, TaskInstanceConst.State.Completing));
                            }
                            //更新当前的任务实例
                            taskInstance.State = TaskInstanceConst.State.Completing;
                            taskInstance.TaskInstanceProcesses.Where(e => e.State != TaskInstanceProcessConst.State.Completing && e.State != TaskInstanceProcessConst.State.Completed).ForEach(dr =>
                            {
                                dr.State = TaskInstanceProcessConst.State.Completed;
                            });
                            _taskInstanceService.Repository.ExecuteUpdate(e => e.Id == taskInstance.Id, s => s.SetProperty(b => b.State, TaskInstanceConst.State.Completing));
                        }
                    }
                    break;
                case TaskState.MissionStateEnum.Failed:
                    if (taskRecordDto.State != TaskRecordConst.State.Canceled)
                    {
                        taskRecordDto.State = TaskRecordConst.State.Faulted;
                        taskInstanceDto.State = TaskInstanceConst.State.Faulted;
                        if (workDto != null)
                        {
                            workDto.State = TaskInstanceConst.State.Faulted;
                        }
                    }
                    break;
            }
            if (request.State == TaskState.MissionStateEnum.Put)
            {
                //放货完成，更新库位状态后直接返回
                return Ok(new
                {
                    missionId = request.MissionId,
                    carCode = car.Code,
                    state = request.State,
                    triggerTime = request.TriggerTime
                });
            }
            if (request.State == TaskState.MissionStateEnum.Fetched)
            {
                //当为Fetched时，只更新taskRecordDto状态
                if (taskRecordDto.State != TaskRecordConst.State.Canceled && taskRecordDto.State != TaskRecordConst.State.Faulted)
                {
                    _taskRecordService.UpdateTaskRecordState(taskRecordDto.Id, TaskRecordConst.State.Putting);
                }
            }
            else if (request.State == TaskState.MissionStateEnum.Put)
            {
                _taskRecordService.Update(taskRecordDto);
            }
            else
            {
                _taskRecordService.Update(taskRecordDto);
                _taskInstanceService.Update(taskInstanceDto);
            }
            if (workDto != null)
            {
                _workService.Update(workDto);
            }
            //重复任务逻辑处理
            if (taskRecordDto.IsLoop && request.State == TaskState.MissionStateEnum.Finished)
            {
                var startNode = _bootService.Nodes.FirstOrDefault(e => e.Id == taskRecordDto.SrcNodeId);
                if (startNode == null)
                {
                    return BadRequest($"获取起点站点失败 [{taskRecordDto.SrcNodeId}]");
                }
                var endNode = _bootService.Nodes.FirstOrDefault(e => e.Id == taskRecordDto.DestNodeId);
                if (endNode == null)
                {
                    return BadRequest($"获取终点站点失败 [{taskRecordDto.DestNodeId}]");
                }
                var dto = new TaskRecordDto
                {
                    TaskTemplateId = taskRecordDto.TaskTemplateId,
                    //CarId = taskRecordDto.CarId,
                    CarTypeId = taskRecordDto.CarTypeId,
                    Code = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Name = taskRecordDto.Name,
                    Type = taskRecordDto.Type,
                    SrcNodeId = taskRecordDto.SrcNodeId,
                    SrcNodeCode = startNode.Code,
                    DestNodeId = taskRecordDto.DestNodeId,
                    DestNodeCode = endNode.Code,
                    Priority = taskRecordDto.Priority,
                    State = TaskRecordConst.State.Created,
                    IsLoop = taskRecordDto.IsLoop,
                    AppendId = taskRecordDto.Id,
                    CallMode = ""
                };
                var result = _taskRecordService.AddTaskRecord(dto, dto.TaskTemplateId);
                if (result == 0)
                {
                    return BadRequest($"任务创建失败");
                }
                try
                {
                    var srcNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = startNode.Code
                    };
                    var destNode = new Service.Models.FlowExtend.TaskReqNode()
                    {
                        Code = endNode.Code
                    };
                    //获取容器的尺寸
                    double width = -1, length = -1, height = -1;
                    var extend = taskRecordDto.Extend?.JsonTo<TaskRecordExtend>();
                    if (extend != null && extend.ContainerSize != null)
                    {
                        width = extend.ContainerSize.Width;
                        length = extend.ContainerSize.Length;
                        height = extend.ContainerSize.Height;
                    }
                    var containerSize = new Service.Models.FlowExtend.ContainerSize()
                    {
                        Width = width,
                        Length = length,
                        Height = height
                    };
                    var req = CarTaskExtension.ToCarTask(dto, srcNode, destNode, containerSize, null, car.CarType.Code);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        dto.State = TaskInstanceConst.State.Released;
                        _taskRecordService.UpdateTaskRecordState(dto.Id, dto.State);
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

            //空满交换取空完成时，发布送满任务
            if (workDto is not null && (workDto.Extend is not null && (workDto.Extend.Contains("EmptyFullExchange") || workDto.Extend.Contains("FullEmptyExchange"))) && request.State == TaskState.MissionStateEnum.Finished)
            {
                var extend = taskRecordDto.Extend?.JsonTo<TaskRecordExtend>();
                if (extend == null || extend.FollowUpTaskId == null)
                {
                    return BadRequest($"空满交换失败 [{taskRecordDto.Id}]");
                }
                //获取待发布任务               
                var releasedtaskRecordDto = _taskRecordService.FirstOrDefault(e => e.Id == extend.FollowUpTaskId);
                if (releasedtaskRecordDto == null)
                {
                    return BadRequest($"空满交换送满任务查找失败 [{taskRecordDto.Id}]");
                }
                var startNode = _bootService.Nodes.FirstOrDefault(e => e.Id == releasedtaskRecordDto.SrcNodeId);
                if (startNode == null)
                {
                    return BadRequest($"获取起点站点失败 [{taskRecordDto.SrcNodeId}]");
                }
                var endNode = _bootService.Nodes.FirstOrDefault(e => e.Id == releasedtaskRecordDto.DestNodeId);
                if (endNode == null)
                {
                    return BadRequest($"获取终点站点失败 [{taskRecordDto.DestNodeId}]");
                }
                //根据任务id获取work中容器
                double width = 0, length = 0, height = 0;
                var work2Dto = _workService.FirstOrDefault(e => e.TaskId == releasedtaskRecordDto.Id);
                if (work2Dto != null)
                {
                    var container = _containerService.Set().FirstOrDefault(e => e.Id == work2Dto.ContainerId);
                    width = container != null ? container.Width : -1;
                    length = container != null ? container.Length : -1;
                    height = container != null ? container.Height : -1;
                }
                var containerSize = new Service.Models.FlowExtend.ContainerSize()
                {
                    Width = width,
                    Length = length,
                    Height = height
                };
                var srcNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(startNode.Code) };
                var destNode = new Service.Models.FlowExtend.TaskReqNode() { Code = Guard.NotNull(endNode.Code) };
                try
                {
                    var req = CarTaskExtension.ToCarTask(releasedtaskRecordDto, srcNode, destNode, containerSize, car, car.CarType.Code);
                    var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                        $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                    );
                    if (resp.Code == 200)
                    {
                        releasedtaskRecordDto.State = TaskInstanceConst.State.Released;
                        _taskRecordService.UpdateTaskRecordState(releasedtaskRecordDto.Id, releasedtaskRecordDto.State);
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
            return Ok(new
            {
                missionId = request.MissionId,
                carCode = car.Code,
                state = request.State,
                triggerTime = request.TriggerTime
            });
        }

        [Tags("任务（mdcs单点）")]
        [HttpPost]
        public IActionResult TaskSingle(Models.Request.TaskSingle request)
        {
            var targetNode = _bootService.Nodes.FirstOrDefault(e => e.Code == request.TargetNodeCode);
            if (targetNode == null)
            {
                return BadRequest($"获取目标站点失败 [{request.TargetNodeCode}]");
            }
            Car? car = null;
            if (string.IsNullOrEmpty(request.CarCode))
            {
                return BadRequest($"单点任务必须选择车辆");
            }
            car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
            if (car == null)
            {
                return BadRequest($"获取车辆失败 [{request.CarCode}]");
            }
            var taskDto = new TaskRecordDto()
            {
                CarId = car.Id,
                Code = car.Code,
                CarTypeId = car.CarTypeId,
                Name = $"车辆 [{request.CarCode}] 去目标站点 [{targetNode.Code}]",
                Type = "Template",
                State = TaskInstanceConst.State.Created,
                SrcNodeId = Guard.NotNull(car.CurrNodeId),
                DestNodeId = targetNode.Id,
                SrcNodeCode = car.CurrNode?.Code,
                DestNodeCode = targetNode.Code,
                TaskTemplateId = "Single",
                CallMode = ""
            };
            var result = _taskRecordService.AddTaskRecord(taskDto, "Single");
            if (result == 0)
            {
                return BadRequest($"任务创建失败");
            }
            var dto = Guard.NotNull(_taskRecordService.ToList(e => e.Id == taskDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefault());
            try
            {
                var req = new
                {
                    CarCode = car.Code,
                    TaskCode = dto.Id,
                    TaskType = dto.TaskTemplateId,
                    Nodes = new List<TaskReqNode>
                    {
                        new TaskReqNode{ Code =Guard.NotNull(car.CurrNode?.Code)},
                        new TaskReqNode{ Code =Guard.NotNull(targetNode.Code)}
                    }
                };
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    _taskRecordService.UpdateTaskRecordState(dto.Id, dto.State);
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

        [Tags("任务（mdcs双点）")]
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
            var taskDto = new TaskRecordDto()
            {
                CarId = car?.Id,
                Code = $"{request.CarCode}=>[{startNode.Code}=>{endNode.Code}]",
                CarTypeId = car?.CarTypeId,
                Name = $"车辆 [{request.CarCode}] 从起点站点 [{startNode.Code}] 到终点站点 [{endNode.Code}]",
                Type = "Template",
                State = TaskInstanceConst.State.Created,
                SrcNodeId = startNode.Id,
                DestNodeId = endNode.Id,
                SrcNodeCode = startNode.Code,
                DestNodeCode = endNode.Code,
                TaskTemplateId = "Double",
                CallMode = ""
            };
            var result = _taskRecordService.AddTaskRecord(taskDto, "Double");
            if (result == 0)
            {
                return BadRequest($"任务创建失败");
            }
            var dto = Guard.NotNull(_taskRecordService.ToList(e => e.Id == taskDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefault());
            try
            {
                var req = new
                {
                    CarCode = car?.Code,
                    TaskCode = dto.Id,
                    TaskType = dto.TaskTemplateId,
                    Nodes = new List<TaskReqNode>
                    {
                        new TaskReqNode{ Code =Guard.NotNull(startNode.Code)},
                        new TaskReqNode{ Code =Guard.NotNull(endNode.Code)}
                    }
                };
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    _taskRecordService.UpdateTaskRecordState(dto.Id, dto.State);
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

        [Tags("任务测试（mdcs满空交换）")]
        [HttpPost]
        public async Task<IActionResult> TaskExchangeTestAsync(Models.Request.TaskExchange request)
        {
            Car? car = null;
            string carType = "Car";//默认车型
            string? carTypeId = null;
            if (!string.IsNullOrEmpty(request.CarCode))
            {
                car = _bootService.Cars.FirstOrDefault(e => e.Code == request.CarCode);
                if (car == null) return BadRequest($"获取车辆失败 [{request.CarCode}]");
            }
            if (!string.IsNullOrEmpty(request.CarType))
            {
                if (!_bootService.Cars.Any(e => e.CarType.Code == request.CarType))
                    return BadRequest($"获取车辆类型 [{request.CarType}]");
            }
            carType = request.CarType ?? carType;//获取车型编码
            carTypeId = _bootService.Cars.Where(e => e.CarType.Code == carType).FirstOrDefault()?.CarTypeId;
            //1、叫料库位是否存在
            var storage = await _storageService.Set().AsNoTracking().FirstOrDefaultAsync(e => e.Code == request.StorageCode && e.IsEnable && !string.IsNullOrEmpty(e.NodeCode));
            if (storage == null)
            {
                return BadRequest($"库位不存在,库位编号 [{request.StorageCode}]");
            }
            if (storage.IsLock)
            {
                return BadRequest($"满空交换失败，叫料工位已被锁定");
            }
            if (storage.State != StorageConst.State.FullContainer)
            {
                return BadRequest($"满空交换失败，叫料工位无满容器");
            }
            //2、叫料工位是否满料
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
            //3、判定满桶中转区有库位且为未锁定状态
            var storageFull = await _storageService.Set().AsNoTracking().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "MTZZQ").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
            if (storageFull == null)
            {
                return BadRequest($"满桶周转区不存在空库位，任务下发失败");
            }
            //4、判断空桶周转区是否有空桶且为未锁定状态
            var storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsLock == false && e.AreaCode == "KTZZQ").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
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
            //5、变更库位锁状态
            storage.IsLock = true;
            storageFull.IsLock = true;
            storageEmpty.IsLock = true;
            //await _storageService.UpdateAsync(new List<StorageDto> { storage, storageFull, storageEmpty });
            //6、下发点对点搬运任务
            var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", storage, storageFull); //取满任务
            taskRecordDto.Code = $"{taskRecordDto.Code}-1";
            taskRecordDto.CarId = car?.Id;//添加车辆任务
            taskRecordDto.CarTypeId = carTypeId;//添加车型
            taskRecordDto.Priority = 1;//第一段任务优先级设置为1
            var followupTaskDto = CarTaskExtension.CreateTaskRecord("Double", storageEmpty, storage);//送空任务
            followupTaskDto.Code = $"{taskRecordDto.Code.Replace("-1", "-2")}";
            followupTaskDto.CarTypeId = carTypeId;//添加车型
            //7、将后置任务记录id存入前置任务Extend字段
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
            //8、添加work记录
            //1)取满桶
            var callModeName = "满桶下线/空桶上线";
            var result = await _workService.AddWorkAsync(storage.AreaId, callModeName + "(下满桶)", taskRecordDto, container, "", "FullEmptyExchange");
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
                var req = CarTaskExtension.ToCarTask(taskRecordDto, startNode, endNode, containerSize, car, carType);
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
            return Ok();
        }

        [Tags("机器人任务")]
        [HttpPost]
        public async Task<IActionResult> RobotTask(Models.Request.RobotTask request)
        {
            _logger.LogInformation($"RobotTask:{request.ToJson()}");
            string carTypeCode = string.Empty;
            if (string.IsNullOrEmpty(request.CarTypeCode))
            {
                carTypeCode = "Car";//默认车型
            }
            else
            {
                carTypeCode = request.CarTypeCode;
            }
            string? carTypeId = _bootService.Cars.Where(e => e.CarType.Code == carTypeCode).FirstOrDefault()?.CarTypeId;
            if (carTypeId is null)
            {
                return BadRequest($"车辆不存在，车辆类型 [{carTypeCode}] ");
            }
            if (string.IsNullOrEmpty(request.StorageCode))
            {
                return BadRequest($"库位编号不能为空 [{request.StorageCode}]");
            }
            //判断叫料库位是否存在
            var storage = await _storageService.FirstOrDefaultAsync(e => e.Code == request.StorageCode && e.IsEnable && !string.IsNullOrEmpty(e.NodeCode));
            if (storage is null)
            {
                return BadRequest($"库位不存在,库位编号 [{request.StorageCode}]");
            }
            //必须机器人工位所在区域才能发机器人任务
            if (storage.AreaCode is not null && !storage.AreaCode.Equals("FYBF"))
            {
                return BadRequest($"库位所在区域编码不正确,库位编号 [{request.StorageCode}]");
            }
            if (!request.Type.Equals("Call") && !request.Type.Equals("Release"))
            {
                return BadRequest($"库位[{request.StorageCode}] 任务类型type:[{request.Type}]不正确");
            }
            StorageDto? startStorageDto;
            StorageDto? endStorageDto;
            if (request.Type.Equals("Call"))
            {
                //呼叫上料任务
                //废药摆放展示区呼叫，起点是废药暂存展示区(FYZC)，终点是当前站点
                startStorageDto = await _storageService.Set().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsEnable && e.IsLock == false && e.AreaCode == "FYZC").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                if (startStorageDto is null)
                {
                    return BadRequest($"废药摆放展示区呼叫失败，废药暂存库位不存在空桶");
                }
                endStorageDto = storage.DeepClone();
            }
            else
            {
                //呼叫放行任务
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

            //1、下发点对点搬运任务
            var callModeName = $"{storage.Name}-空桶上线";//拼接工位-模板名称
            TaskRecordDto taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", startStorageDto, endStorageDto); //取空任务
            taskRecordDto.CallMode = request.CallModel;
            taskRecordDto.CarTypeId = carTypeId;
            //2、判定库位容器绑定关系
            var containers = await _storageService.GetContainersAsync(startStorageDto);
            if (containers == null || containers.Count() == 0)
            {
                return BadRequest($"起点工位[{startStorageDto.Name}]没有容器，任务下发失败");
            }
            var container = containers.First();
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

        [Tags("全局报警上报")]
        [HttpPost]
        public async Task<IActionResult> AlarmReporting(Models.Request.GlobalAlarm request)
        {
            await _capPublisher.PublishAsync("MdcsController.GlobalAlarm", $"报警等级:{request.Level},描述:{request.alarmInfo}");
            return Ok();
        }

    }

}