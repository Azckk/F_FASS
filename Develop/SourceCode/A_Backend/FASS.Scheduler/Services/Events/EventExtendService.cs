using Common.AspNetCore.Extensions;
using Common.Frame.Services.Cache.Interfaces;
using Common.Frame.Services.Frame.Interfaces;
using Common.NETCore.Extensions;
using FASS.Boot.Services;
using FASS.Boot.Services.Archives;
using FASS.Boot.Services.Cars;
using FASS.Data.Consts.Base;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Flow;
using FASS.Data.Models.Base;
using FASS.Data.Models.Data;
using FASS.Data.Models.Flow;
using FASS.Data.Models.Instant;
using FASS.Scheduler.Services.Cars.Fairyland.Plc;
using FASS.Scheduler.Services.Events.Models;
using FASS.Scheduler.Utility;
using FASS.Service.Models.RecordExtend;
using FASS.Service.Services.RecordExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Scheduler.Services.Events
{
    public class EventExtendService
    {
        public EventService EventService { get; }

        public IBootService BootService { get; }

        private readonly IDistributedCache _distributedCache;
        private readonly IAlarmMdcsService _alarmMdcsService;
        private readonly IConfigService _configService;
        public bool IsStarted { get; private set; }
        private int _alarmDuration = 3000;

        public EventExtendService(
            EventService eventService)
        {
            EventService = eventService;

            BootService = EventService.ServiceProvider.GetRequiredService<IBootService>();

            _alarmMdcsService = EventService.ServiceProvider.GetScopeService<IAlarmMdcsService>();
            _distributedCache = EventService.ServiceProvider.GetScopeService<IDistributedCache>();
            _configService = EventService.ServiceProvider.GetScopeService<IConfigService>();

            Init();
        }

        public void Init()
        {
            AlarmHandle();
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
                SetStart();
            }
            catch (Exception ex)
            {
                EventService.Logger.LogError(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                if (IsStarted)
                {
                    return;
                }
                IsStarted = false;
                SetStop();
            }
            catch (Exception ex)
            {
                EventService.Logger.LogError(ex.ToString());
            }
        }

        private void SetStart()
        {
            //归档服务定时执行后触发
            BootService.ArchiveServiceElapsed += async (ArchiveService service) =>
            {
                await EventService.ServiceProvider.GetScopeService<IWorkService>().DeleteDayAsync(day: EventService.ServiceProvider.GetScopeService<IDataService>().GetConfigToValue("WorkDayLimit", "WorkDayLimit").ToInt());

                await EventService.ServiceProvider.GetScopeService<IChargeConsumeService>().AddOrUpdateAsync(BootService.Cars);
                await EventService.ServiceProvider.GetScopeService<IDisChargeConsumeService>().AddOrUpdateAsync(BootService.Cars);
            };

            //车辆服务定时执行后触发
            BootService.CarServiceElapsed += (CarService carService) =>
            {
                var car = carService.Car;
                //var carMinBattery = EventService.ServiceProvider.GetScopeService<IDataService>().GetConfigToValue("CarMinBattery", "CarMinBattery").ToInt();
                var carMinBattery = 30;
                if (car.Battery < car.MinBattery || car.Battery < carMinBattery)
                {
                    if (car.CarCharges.Any() && car.CurrTaskInstance?.Type != TaskInstanceConst.Type.Charge && car.NextTaskInstance?.Type != TaskInstanceConst.Type.Charge)
                    {
                        if (BootService.TryGetChargeTaskInstance(car, out var result) && result is not null)
                        {
                            BootService.AddTaskInstance(result);
                            return;
                        }
                    }
                }
                if (car.CarStandbys.Any() && car.CurrTaskInstance == null && car.NextTaskInstance == null)
                {
                    if (car.CarStandbys.Any(e => e.Standby.NodeId == car.CurrNodeId))
                    {
                        return;//已经在待命点
                    }
                    var isIdle = true;
                    if (car.Type == "Fairyland.Pc")
                    {
                        if (car.CarState is not null && car.CarState.TryJsonTo<Extend.Car.Fairyland.Pc.Models.Response.CarState>(out var carState) && carState != null)
                        {
                            isIdle = carState.HoldingLocks.Count == 1 ? true : false;
                        }
                    }
                    if (BootService.TryGetStandbyTaskInstance(car, out var result) && isIdle && result is not null)
                    {
                        if (result.Nodes?.First() != car.CurrNodeId)
                        {
                            BootService.AddTaskInstance(result);
                            return;
                        }
                    }
                }
            };

            //任务实例查找车时触发
            BootService.TryTaskInstanceFindCar += (TaskInstance taskInstance, Node node, ref Car? car) =>
            {
                var carMinBattery = EventService.ServiceProvider.GetScopeService<IDataService>().GetConfigToValue("CarMinBattery", "CarMinBattery").ToInt();
                Func<Car, bool> predicate = e => (taskInstance.CarTypeId == null || e.CarTypeId == taskInstance.CarTypeId) && e.CurrTaskInstance == null && e.Battery > e.MinBattery && e.Battery > carMinBattery;
                if (!BootService.TryGetCar(node, predicate, out car))
                {
                    return false;
                }
                return true;
            };

            //获取规划路径时触发
            //EventService.BootService.TryGetPlanRouteExecute += (Car car, Node startNode, Node endNode, ref Queue<Edge> result) =>
            //{
            //    var node = EventService.BootService.Nodes.FirstOrDefault(e => e.Code == "141");
            //    if (node?.IsLock is true)
            //    {
            //        if (!node.NodeExtends.Any(e => e.Key == EdgeConst.Extend.CarLock))
            //        {
            //            var nodeExtend = new NodeExtend();
            //            nodeExtend.Key = EdgeConst.Extend.CarLock;
            //            nodeExtend.Value = new[] { new { CarCode = "1", IsLock = false } }.ToJson();
            //            node.NodeExtends.Add(nodeExtend);
            //        }
            //    }
            //    return null;
            //};

            //获取规划路径遍历路线时触发
            BootService.TryGetPlanNextEdgeExecute += (Car car, Edge nextEdge, ref Edge result) =>
            {
                var planResult = !nextEdge.IsLock;
                var nextEdgeCarLockExtend = nextEdge.EdgeExtends.FirstOrDefault(e => e.Key == EdgeConst.Extend.CarLock);
                if (nextEdgeCarLockExtend is not null)
                {
                    var nextEdgeCarLockValues = nextEdgeCarLockExtend.Value?.JsonTo<List<CarLock>>();
                    var nextEdgeCarLockValue = nextEdgeCarLockValues?.FirstOrDefault(e => e.CarCode == car.Code);
                    if (nextEdgeCarLockValue is not null)
                    {
                        planResult = !nextEdgeCarLockValue.IsLock;
                    }
                }
                return planResult;
            };

            //获取规划路径遍历站点时触发
            BootService.TryGetPlanNextNodeExecute += (Car car, Node nextNode, ref Node result) =>
            {
                //var nextNodeResult = !nextNode.IsLock || !nextNode.Zones.Any(e => e.IsLock);
                var nextNodeResult = !nextNode.IsLock;
                var nextNodeCarLockExtend = nextNode.NodeExtends.FirstOrDefault(e => e.Key == NodeConst.Extend.CarLock);
                if (nextNodeCarLockExtend is not null)
                {
                    var nextNodeCarLockValues = nextNodeCarLockExtend.Value?.JsonTo<List<CarLock>>();
                    var nextNodeCarLockValue = nextNodeCarLockValues?.FirstOrDefault(e => e.CarCode == car.Code);
                    if (nextNodeCarLockValue is not null)
                    {
                        nextNodeResult = !nextNodeCarLockValue.IsLock;
                    }
                }
                return nextNodeResult;
            };

            //获取避让规划路径遍历路线时触发
            BootService.TryGetAvoidPlanNextEdgeExecute += (Car car, Edge nextEdge, ref Edge result) =>
            {
                var nextEdgeResult = !((nextEdge.StartNode.IsOccupy && nextEdge.StartNode.OccupyCar != car) || (nextEdge.EndNode.IsOccupy && nextEdge.EndNode.OccupyCar != car));
                return nextEdgeResult;
            };

            //获取避让规划路径遍历站点时触发
            BootService.TryGetAvoidPlanNextNodeExecute += (Car car, Node nextNode, ref Node result) =>
            {
                var nextNodeResult = !(nextNode.IsOccupy && nextNode.OccupyCar != car);
                return nextNodeResult;
            };

            //车辆即时动作执行时触发
            BootService.TryCarInstantActionExecute += (Car car, ref CarInstantAction carInstantAction) =>
            {
                if (carInstantAction is not null)
                {
                    EventService.Logger.LogDebug($"车辆编码：[{car.Code}] 动作类型：[{carInstantAction.ActionType}]");
                }
                return false;
            };

            //任务实例执行时触发
            BootService.TryTaskInstanceExecute += (Car car, ref TaskInstance taskInstance) =>
            {
                //var taskInstanceRef = taskInstance;
                //var work = _workService.FirstOrDefault(e => e.TaskId == taskInstanceRef.Id);
                //if (work != null && work.State != taskInstanceRef.State)
                //{
                //    _workService.ExecuteUpdate(e => e.Id == work.Id, e => e.State, taskInstanceRef.State);
                //}
                return false;
            };

            //任务实例停止时触发
            BootService.TryTaskInstanceStop += (Car car, ref TaskInstance taskInstance) =>
            {
                if (taskInstance.TaskTemplate?.IsLoop is true)
                {
                    if (BootService.TryGetAppendTaskInstance(taskInstance, out var result))
                    {
                        BootService.AddTaskInstance(result);
                    }
                    return false;
                }
                return false;
            };

            //任务实例子任务动作进行中触发
            BootService.TryTaskInstanceActionRunning += (Car car, ref TaskInstanceAction taskInstanceAction) =>
            {
                if (taskInstanceAction.ActionType == CarActionConst.Type.StartCharging)
                {
                    //打开充电机构控制逻辑
                    //taskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                }
                if (taskInstanceAction.ActionType == CarActionConst.Type.StopCharging)
                {
                    var carMaxBattery = EventService.ServiceProvider.GetScopeService<IDataService>().GetConfigToValue("CarMaxBattery", "CarMaxBattery").ToInt();
                    if (car.Battery >= car.MaxBattery && car.Battery >= carMaxBattery)
                    {
                        //关闭充电机构控制逻辑
                        //taskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                    }
                }
                if (taskInstanceAction.ActionType == CarActionConst.Type.StartPause)
                {
                    var taskId = taskInstanceAction.TaskInstanceProcess.TaskInstanceId;
                    var work = EventService.ServiceProvider.GetScopeService<IWorkService>().FirstOrDefault(e => e.TaskId == taskId);
                    if (work is not null)
                    {
                        var container = EventService.ServiceProvider.GetScopeService<IContainerService>().FirstOrDefault(e => e.Id == work.ContainerId);
                        if (container?.AreaCode == "A")
                        {
                            var materials = EventService.ServiceProvider.GetScopeService<IContainerService>().GetMaterials(container);
                            var storages = EventService.ServiceProvider.GetScopeService<IMaterialService>().GetStorages(materials);
                            if (!storages.Any(e => e.NodeId == car.CurrNodeId))
                            {
                                taskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                            }
                        }
                    }
                }
                return false;
            };

            //任务实例取消前触发
            BootService.TryTaskInstanceCanceling += (Car car, ref TaskInstance taskInstance) =>
            {
                var carService = BootService.CarServices.FirstOrDefault(e => e.Car == car);
                if (carService != null)
                {
                    (carService.CarSessionService as CarSessionService)?.SendControlStop();
                }
                return false;
            };

            //任务实例暂停前触发
            BootService.TryTaskInstancePausing += (Car car, ref TaskInstance taskInstance) =>
            {
                var carService = BootService.CarServices.FirstOrDefault(e => e.Car == car);
                if (carService != null)
                {
                    (carService.CarSessionService as CarSessionService)?.SendControlStop();
                }
                return false;
            };

            //任务实例恢复前触发
            BootService.TryTaskInstanceResuming += (Car car, ref TaskInstance taskInstance) =>
            {
                var carService = BootService.CarServices.FirstOrDefault(e => e.Car == car);
                if (carService != null)
                {
                    (carService.CarSessionService as CarSessionService)?.SendControlStart();
                }
                return false;
            };

            //车辆当前站点变更时触发
            BootService.CarCurrNodeChanged += (Car prevCar, Car currCar) =>
            {
                //if (currCar.CurrNodeId != prevCar?.CurrNodeId && prevCar!=  null)
                //{
                //    EventService.Logger.LogWarning($"currNode => {currCar.CurrNode.Code}");
                //}
            };
        }

        private void SetStop()
        {
            //归档服务定时执行后触发
            BootService.ArchiveServiceElapsed = null!;

            //车辆服务定时执行后触发
            BootService.CarServiceElapsed = null!;

            //任务实例查找车时触发
            BootService.TryTaskInstanceFindCar = null!;

            //获取规划路径时触发
            //EventService.BootService.TryGetPlanRouteExecute = null;

            //获取规划路径遍历路线时触发
            BootService.TryGetPlanNextEdgeExecute = null!;

            //获取规划路径遍历站点时触发
            BootService.TryGetPlanNextNodeExecute = null!;

            //获取避让规划路径遍历路线时触发
            BootService.TryGetAvoidPlanNextEdgeExecute = null!;

            //获取避让规划路径遍历站点时触发
            BootService.TryGetAvoidPlanNextNodeExecute = null!;

            //车辆即时动作执行时触发
            BootService.TryCarInstantActionExecute = null!;

            //任务实例执行时触发
            BootService.TryTaskInstanceExecute = null!;

            //任务实例停止时触发
            BootService.TryTaskInstanceStop = null!;

            //任务实例子任务动作进行中触发
            BootService.TryTaskInstanceActionRunning = null!;

            //任务实例取消前触发
            BootService.TryTaskInstanceCanceling = null!;

            //任务实例暂停前触发
            BootService.TryTaskInstancePausing = null!;

            //任务实例恢复前触发
            BootService.TryTaskInstanceResuming = null!;

            //车辆当前站点变更时触发
            BootService.CarCurrNodeChanged = null!;
        }

        public void AlarmHandle()
        {
            //初始化参数
            var configDto = _configService.Set().Where(e => e.Key == "AlarmTriggerDuration").FirstOrDefault();
            _alarmDuration = configDto == null ? _alarmDuration : configDto.Value.ToInt();
            //获取缓存数据
            var temp = _distributedCache.GetString("PcAlarm.cartAlarmCodeMap")?.JsonTo<List<KeyValuePair<string, Dictionary<string, string>>>>();
            if (temp == null)
            {
                cartAlarmCodeMap = new Dictionary<string, Dictionary<string, string>>();
            }
            else
            {
                cartAlarmCodeMap = temp.ToDictionary(item => item.Key, item => item.Value);
            }
            var temp2 = _distributedCache.GetString("PcAlarm.alarmTriggerRecord")?.JsonTo<List<KeyValuePair<string, Dictionary<string, DateTime>>>>();
            if (temp2 == null)
            {
                alarmTriggerRecord = new Dictionary<string, Dictionary<string, DateTime>>();
            }
            else
            {
                alarmTriggerRecord = temp2.ToDictionary(item => item.Key, item => item.Value);
            }
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var last = DateTime.Now;
                while (true)
                {
                    if ((DateTime.Now - last).TotalMilliseconds >= 2000)
                    {
                        last = DateTime.Now;
                        scanAlarm();//报警记录判定
                    }
                    Thread.Sleep(100);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private Dictionary<string, Dictionary<string, string>> cartAlarmCodeMap = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, DateTime>> alarmTriggerRecord = new Dictionary<string, Dictionary<string, DateTime>>();
        List<AlarmMdcs> modelAddList = new List<AlarmMdcs>();
        List<AlarmMdcs> modelUpdateList = new List<AlarmMdcs>();
        private object alarmLock = new object();
        private void scanAlarm()
        {
            try
            {
                //获取所有Pc类型小车
                var cars = BootService.Cars.Where(e => e.Type == "Fairyland.Pc").ToList();
                foreach (var car in cars)
                {
                    if (car.CarState is null || !car.CarState.TryJsonTo<FASS.Extend.Car.Fairyland.Pc.Models.Response.CarState>(out var carState) || carState == null)
                    {
                        EventService.Logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 车辆状态错误");
                        continue;
                    }
                    var alarmCodes = carState.Alarms.ToDictionary(item => item.Code, item => item.Name);
                    if (cartAlarmCodeMap.ContainsKey(car.Code))
                    {
                        var last = cartAlarmCodeMap[car.Code];
                        // 扫描解除的报警
                        if (alarmTriggerRecord.ContainsKey(car.Code))
                        {
                            foreach (var alarmCode in alarmTriggerRecord[car.Code].Keys)
                            {
                                var triggerCase = !alarmCodes.ContainsKey(alarmCode);
                                CommonHelper.TriggerOnce(triggerCase, _alarmDuration, () =>
                                {
                                    lock (alarmLock)
                                    {
                                        var mode = new AlarmMdcs
                                        {
                                            CarCode = car.Code,
                                            Code = alarmCode,
                                            EndTime = DateTime.Now
                                        };
                                        EventService.Logger.LogWarning($"报警判定服务 => {mode.CarCode} || {mode.Code}");
                                        modelUpdateList.Add(mode);
                                        //_alarmMdcsService.UpdateModel(mode);

                                        alarmTriggerRecord[car.Code].Remove(alarmCode);
                                        if (alarmTriggerRecord[car.Code].Keys.Count == 0)
                                        {
                                            alarmTriggerRecord.Remove(car.Code);
                                        }
                                    }
                                }, car.Code.ToInt(), $"{car.Code}=>{alarmCode}");
                            }
                        }
                        else
                        {
                            //不在触发记录缓存中，直接根据根据上一个点的报警状态和当前报警状态来结束报警(原逻辑)
                            foreach (var alarmCode in last.Keys)
                            {
                                if (!alarmCodes.ContainsKey(alarmCode))
                                {
                                    lock (alarmLock)
                                    {
                                        var mode = new AlarmMdcs
                                        {
                                            CarCode = car.Code,
                                            Code = alarmCode,
                                            EndTime = DateTime.Now
                                        };
                                        modelUpdateList.Add(mode);
                                        //_alarmMdcsService.UpdateModel(mode);
                                    }
                                }
                            }
                        }
                        // 扫描触发的报警
                        foreach (var alarmCode in alarmCodes.Keys)
                        {
                            var triggerCase = last.ContainsKey(alarmCode);
                            CommonHelper.TriggerOnce(triggerCase, _alarmDuration, () =>
                            {
                                if (alarmTriggerRecord.ContainsKey(car.Code))
                                {
                                    if (!alarmTriggerRecord[car.Code].ContainsKey(alarmCode))
                                    {
                                        lock (alarmLock)
                                        {
                                            alarmTriggerRecord[car.Code].Add(alarmCode, DateTime.Now);
                                            var mode = new AlarmMdcs
                                            {
                                                CarCode = car.Code,
                                                CarName = car.Name,
                                                Code = alarmCode,
                                                Name = alarmCodes[alarmCode],
                                                StartTime = DateTime.Now
                                            };
                                            modelAddList.Add(mode);
                                            //_alarmMdcsService.AddModel(mode);
                                            //Thread.Sleep(20);
                                        }
                                    }
                                }
                                else
                                {
                                    lock (alarmLock)
                                    {
                                        if (!alarmTriggerRecord.ContainsKey(car.Code))
                                        {
                                            alarmTriggerRecord.Add(car.Code, new Dictionary<string, DateTime>() { { alarmCode, DateTime.Now } });
                                        }
                                        else
                                        {
                                            alarmTriggerRecord[car.Code].Add(alarmCode, DateTime.Now);
                                        }
                                        var mode = new AlarmMdcs
                                        {
                                            CarCode = car.Code,
                                            CarName = car.Name,
                                            Code = alarmCode,
                                            Name = alarmCodes[alarmCode],
                                            StartTime = DateTime.Now
                                        };
                                        modelAddList.Add(mode);
                                        //_alarmMdcsService.AddModel(mode);
                                        //Thread.Sleep(20);
                                    }
                                }
                            }, car.Code.ToInt(), $"{car.Code}=>{alarmCode}");
                        }
                        cartAlarmCodeMap[car.Code] = alarmCodes;
                    }
                    else
                    {
                        cartAlarmCodeMap.Add(car.Code, alarmCodes);
                    }
                }
                //批量更新
                if (modelUpdateList.Count > 0)
                {
                    var temp = modelUpdateList;
                    _alarmMdcsService.UpdateModels(temp);
                    modelUpdateList.RemoveAll(temp.Contains);
                    //_alarmMdcsService.UpdateModels(modelUpdateList);
                    //modelUpdateList.Clear();
                }
                //批量插入
                if (modelAddList.Count > 0)
                {
                    var temp = modelAddList;
                    _alarmMdcsService.AddModels(temp);
                    modelAddList.RemoveAll(temp.Contains);
                    //modelAddList.Clear();
                }
                //存入redis
                _distributedCache.SetString("PcAlarm.cartAlarmCodeMap", cartAlarmCodeMap.ToList().ToJson());
                _distributedCache.SetString("PcAlarm.alarmTriggerRecord", alarmTriggerRecord.ToList().ToJson());
            }
            catch (Exception ex)
            {
                EventService.Logger.LogWarning($"报警判定服务[scanAlarm]执行错误 => Exception:{ex}");
            }
        }

    }
}
