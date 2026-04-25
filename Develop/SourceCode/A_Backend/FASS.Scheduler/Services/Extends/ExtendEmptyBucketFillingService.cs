using Common.AspNetCore.Extensions;
using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Warehouse;
using FASS.Scheduler.Controllers.Extensions;
using FASS.Scheduler.Utility;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FASS.Scheduler.Services.Extends
{
    /*
     * 成都叉车空桶缓存区补位、预定任务自动发送
     */
    public class ExtendEmptyBucketFillingService
    {
        public ExtendService ExtendService { get; }

        public bool IsStarted { get; private set; }

        private readonly IWorkService _workService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IPreWorkService _preWorkService;
        private readonly IMaterialService _materialService;
        public int FillingDueTime { get; set; }
        public int TaskSendDueTime = 60000;

        public ExtendEmptyBucketFillingService(
            ExtendService eventService)
        {
            ExtendService = eventService;

            _workService = ExtendService.ServiceProvider.GetScopeService<IWorkService>();
            _taskRecordService = ExtendService.ServiceProvider.GetScopeService<ITaskRecordService>();
            _storageService = ExtendService.ServiceProvider.GetScopeService<IStorageService>();
            _containerService = ExtendService.ServiceProvider.GetScopeService<IContainerService>();
            _preWorkService = ExtendService.ServiceProvider.GetScopeService<IPreWorkService>();
            _materialService = ExtendService.ServiceProvider.GetScopeService<IMaterialService>();

            Init();
        }

        public void Init()
        {
            FillingDueTime = ExtendService.AppSettings.Extend.FillingDueTime;
        }

        public void Start()
        {
            try
            {
                if (IsStarted)
                {
                    return;
                }
                IsStarted = true;
                FillingService();
                TaskSendService();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                if (!IsStarted)
                {
                    return;
                }
                IsStarted = false;
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void FillingService()
        {
            Task.Factory.StartNew(() =>
            {
                var last = DateTime.Now;
                while (IsStarted)
                {
                    if ((DateTime.Now - last).TotalMilliseconds >= FillingDueTime)
                    {
                        last = DateTime.Now;
                        FillingHandler();
                    }
                    Thread.Sleep(1000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void FillingHandler()
        {
            try
            {
                var storageDtos = _storageService.Set().Where(e => e.IsLock == false && e.IsEnable && e.AreaCode != null && e.AreaCode.Contains("KTHCQ") && e.State == StorageConst.State.NoneContainer).OrderBy(e => e.SortNumber).ToList();//缓存区空库位
                //空桶周转区空桶信息
                var storageEmptyDtos = _storageService.Set().Where(e => e.IsLock == false && e.IsEnable && e.AreaCode == "KTZZQ" && e.State == StorageConst.State.EmptyContainer).OrderByDescending(e => e.SortNumber).ToList();//优先取外侧大编号
                if (storageDtos.Count() > 0 && storageEmptyDtos.Count() > 0)
                {
                    //空桶缓存位需要补位且空桶中转区有空桶(默认一次补一个空桶)
                    //获取空容器
                    var emptyContainers = _storageService.GetContainers(storageEmptyDtos[0]);
                    if (emptyContainers == null || emptyContainers.Count() != 1)
                    {
                        ExtendService.Logger.LogError($"空桶周转区库位{storageEmptyDtos[0].Name} 库位容器绑定关系错误");
                        //库位状态异常时,可锁定空桶缓存区库位
                        return;
                    }
                    var container = emptyContainers.First();
                    //清空容器绑定关系        
                    _containerService.MaterialDelete(container.Id);
                    //锁定起始库位和终点库位
                    //_storageService.Repository.ExecuteUpdate(e => new List<StorageDto> { storageEmptyDtos[0], storageDtos[0] }.Select(e => e.Id).Contains(e.Id), s => s.SetProperty(b => b.IsLock, true));
                    //创建taskRecord
                    var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", storageEmptyDtos[0], storageDtos[0]); //取空任务
                    var callModeName = $"{storageDtos[0].Name}-空桶补位";
                    var result = _workService.AddWorkAsync(storageDtos[0].AreaId, callModeName, taskRecordDto, container, "").Result;//改成同步方法
                    if (result == 0)
                    {
                        return;
                    }
                    //自动发布任务
                    var dto = Guard.NotNull(_taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefault());
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
                        var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null);
                        var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                            $"{ExtendService.AppSettings.Mdcs.SimpleUrl}/car/carTask", req
                        );
                        if (resp.Code == 200)
                        {
                            dto.State = TaskInstanceConst.State.Released;
                            _taskRecordService.UpdateTaskRecordState(dto.Id, dto.State);
                            //更具taskid更新work的状态
                            var workDto = _workService.FirstOrDefault(e => e.TaskId == dto.Id);
                            if (workDto != null)
                            {
                                workDto.State = TaskInstanceConst.State.Released;
                                _workService.Repository.ExecuteUpdate(e => e.Id == workDto.Id, s => s.SetProperty(b => b.State, workDto.State));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExtendService.Logger.LogError($"任务发布失败 =>{ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError($"fail to create empty bucket filling task =>{ex}");
            }
        }

        public void TaskSendService()
        {
            Task.Factory.StartNew(() =>
            {
                var last = DateTime.Now;
                while (IsStarted)
                {
                    if ((DateTime.Now - last).TotalMilliseconds >= TaskSendDueTime)
                    {
                        last = DateTime.Now;
                        TaskHandler();
                    }
                    Thread.Sleep(10000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void TaskHandler()
        {
            /*
             * 1.加载已预定但未成功直送到机台的任务
             * 2.按机台分组取第一个预定任务进行发送(按预定时间升序） 
             */
            var preTaskDtos = _preWorkService.Set()
                .Where(e => e.State == WorkConst.State.Released)
                .GroupBy(e => e.DestStorageId)
                .Select(groups => groups.OrderBy(x => x.CreateAt).FirstOrDefault())
                .ToList();
            if (preTaskDtos is not null)
            {
                foreach (var preTask in preTaskDtos)
                {
                    /**
                     * 根据物料信息，找到当前的满桶库位信息
                     * 根据任务信息，得到送达机台库位信息
                     * 发送搬运任务
                     * */
                    //1.找到物料
                    var materialDto = _materialService.Set().FirstOrDefault(e => preTask != null && e.Code == preTask.MaterialCode && e.IsEnable);
                    if (materialDto is null)
                    {
                        ExtendService.Logger.LogError($"满桶周转区物料信息[{preTask?.MaterialCode}]不存在,自动发送预定任务失败");
                        continue;
                    }
                    //2.找到容器
                    var containers = _materialService.GetContainers(materialDto);
                    if (containers == null || containers.Count() == 0)
                    {
                        ExtendService.Logger.LogError($"满桶周转区物料信息[{preTask?.MaterialCode}]未关联容器,自动发送预定任务失败");
                        continue;
                    }
                    var container = containers.First();
                    //3、找到库位
                    var storages = _containerService.GetStorages(container);
                    if (storages == null || storages.Count() == 0)
                    {
                        ExtendService.Logger.LogError($"满桶周转区物料[{preTask?.MaterialCode}] 容器[{container.Code}] 未绑定库位,自动发送预定任务失败");
                        continue;
                    }
                    var storageSrc = storages.First();
                    //4、判定起点库位是否锁定
                    if (storageSrc.IsLock)
                    {
                        ExtendService.Logger.LogError($"满桶周转区库位[{storageSrc.Name}]已被锁定,自动发送预定任务失败");
                        continue;
                    }
                    //5、送达机台库位
                    var storageDest = _storageService.Set().Where(e => preTask != null && e.Id == preTask.DestStorageId && e.IsLock == false && e.IsEnable && e.State == StorageConst.State.NoneContainer).FirstOrDefault();
                    if (storageDest is null)
                    {
                        ExtendService.Logger.LogError($"预定机台[{preTask?.DestStorageId}]不存在/锁定/有容器,自动发送预定任务失败");
                        continue;
                    }
                    //6、创建任务
                    var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", storageSrc, storageDest);
                    var callModeName = $"{storageDest.Name}-预定任务";
                    var result = _workService.AddWorkAsync(storageDest.AreaId, callModeName, taskRecordDto, container, "").Result;
                    if (result == 0)
                    {
                        ExtendService.Logger.LogError($"预定机台[{storageDest.Name}] ,物料信息[{materialDto.Code}] 任务添加失败!");
                        continue;
                    }
                    //清空任务预定表信息
                    _preWorkService.Delete(e => e.MaterialCode == materialDto.Code && e.DestStorageId == storageDest.Id);
                    //7、自动发布任务
                    var dto = Guard.NotNull(_taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefault());
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
                        var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null);
                        var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                            $"{ExtendService.AppSettings.Mdcs.SimpleUrl}/car/carTask", req
                        );
                        if (resp.Code == 200)
                        {
                            dto.State = TaskInstanceConst.State.Released;
                            _taskRecordService.UpdateTaskRecordState(dto.Id, dto.State);
                            //更具taskid更新work的状态
                            var workDto = _workService.FirstOrDefault(e => e.TaskId == dto.Id);
                            if (workDto != null)
                            {
                                workDto.State = TaskInstanceConst.State.Released;
                                _workService.Repository.ExecuteUpdate(e => e.Id == workDto.Id, s => s.SetProperty(b => b.State, workDto.State));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExtendService.Logger.LogError($"任务发布失败 =>{ex}");
                    }

                    Thread.Sleep(200);
                }
            }
        }

    }
}
