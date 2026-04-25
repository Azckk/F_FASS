using Common.AspNetCore.Extensions;
using Common.NETCore;
using Common.NETCore.Models;
using FASS.Boot.Services;
using FASS.Scheduler.Services.Events.Models;
using FASS.Scheduler.Utility;
using FASS.Service.Models.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendMdcsTaskService
    {
        public ExtendService ExtendService { get; }

        public IBootService BootService { get; }

        public bool IsStarted { get; private set; }


        private readonly ITaskRecordService _taskRecordService;
        public int MdcsTaskSyncDueTime { get; set; }

        public ExtendMdcsTaskService(
            ExtendService eventService)
        {
            ExtendService = eventService;

            BootService = ExtendService.ServiceProvider.GetRequiredService<IBootService>();

            _taskRecordService = ExtendService.ServiceProvider.GetScopeService<ITaskRecordService>();

            Init();
        }

        public void Init()
        {
            MdcsTaskSyncDueTime = ExtendService.AppSettings.Extend.MdcsTaskSyncDueTime;
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
                taskDic.Clear();
                TaskSyncService();
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
                taskDic.Clear();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void TaskSyncService()
        {
            Task.Factory.StartNew(() =>
            {
                var last = DateTime.Now;
                while (IsStarted)
                {
                    if ((DateTime.Now - last).TotalMilliseconds >= MdcsTaskSyncDueTime)
                    {
                        last = DateTime.Now;
                        TaskSyncHandler();
                    }
                    Thread.Sleep(1000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private Dictionary<string, MdcsTask> taskDic = new Dictionary<string, MdcsTask>();
        public void TaskSyncHandler()
        {
            var tasks = new List<MdcsTask>();
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult<List<MdcsTask>>>(
                    $"{ExtendService.AppSettings.Mdcs.SimpleUrl}/task/getTask"
                );
                if (resp.Code == 200)
                {
                    tasks = resp.Data;
                    if (tasks != null && tasks.Count > 0)
                    {
                        List<TaskRecord> addModelList = new List<TaskRecord>();
                        List<TaskRecord> updateModelList = new List<TaskRecord>();
                        foreach (var task in tasks)
                        {
                            var car = BootService.Cars
                                .Where(e => e.Code == task.CarId).FirstOrDefault();
                            var nodeSrc = Guard.NotNull(BootService.Nodes
                                .Where(e => e.Code == task.SrcSiteId).FirstOrDefault());
                            var nodeDest = Guard.NotNull(BootService.Nodes
                                .Where(e => e.Code == task.DestSiteId).FirstOrDefault());
                            //判定任务状态，并更新
                            if (taskDic.ContainsKey(task.TaskId))
                            {
                                if (task.State != taskDic[task.TaskId].State
                                    || task.StartTime != taskDic[task.TaskId].StartTime)
                                {
                                    updateModelList.Add(new TaskRecord
                                    {
                                        Id = task.TaskId,
                                        TaskTemplateId = "Double",
                                        CarId = car?.Id,
                                        CarTypeId = car?.CarTypeId,
                                        Code = $"{task.SrcSiteId}=>{task.DestSiteId}",
                                        Name = task.Name,
                                        Type = "Template",//暂时固化位模板
                                        SrcNodeId = Guard.NotNull(nodeSrc?.Id),
                                        SrcNodeCode = nodeSrc?.Code,
                                        DestNodeId = Guard.NotNull(nodeDest?.Id),
                                        DestNodeCode = nodeDest?.Code,
                                        Priority = task.Priority,
                                        State = task.State,
                                        StartTime = task.StartTime,
                                        EndTime = task.EndTime,
                                        TaskCreateAt = task.Created
                                    });
                                }
                            }
                            else
                            {
                                addModelList.Add(new TaskRecord()
                                {
                                    Id = task.TaskId,
                                    TaskTemplateId = "Double",
                                    CarId = car?.Id,
                                    CarTypeId = car?.CarTypeId,
                                    Code = $"{task.SrcSiteId}=>{task.DestSiteId}",
                                    Name = task.Name,
                                    Type = "Template",//暂时固化位模板
                                    SrcNodeId = Guard.NotNull(nodeSrc?.Id),
                                    DestNodeId = Guard.NotNull(nodeDest?.Id),
                                    SrcNodeCode = nodeSrc?.Code,
                                    DestNodeCode = nodeDest?.Code,
                                    Priority = task.Priority,
                                    State = task.State,
                                    StartTime = task.StartTime,
                                    EndTime = task.EndTime,
                                    TaskCreateAt = task.Created
                                });
                            }
                            taskDic[task.TaskId] = task;
                        }

                        //批量处理数据
                        if (addModelList.Count > 0)
                        {
                            _taskRecordService.AddModelsWithOutTaskInstance(addModelList);
                        }
                        if (updateModelList.Count > 0)
                        {
                            _taskRecordService.UpdateModelsWithOutTaskInstance(updateModelList);
                        }
                    }
                }
                else
                {
                    ExtendService.Logger.LogError($"get: /task/getTask =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError($"fail to get mdcs task =>{ex}");
            }
        }

    }
}
