using Common.AspNetCore.Extensions;
using Common.Frame.Dtos.Frame;
using Common.NETCore.Extensions;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Base;
using FASS.Data.Dtos.Setting;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Flow.Interfaces;
using FASS.Data.Services.Record.Interfaces;
using FASS.Service.Consts.Data;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.RecordExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Hubs.Monitor.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Hubs.Monitor
{
    public class RuntimeHub : Hub
    {
        private readonly ILogger<RuntimeHub> _logger;
        private readonly ICarService _carService;
        private readonly IAlarmService _alarmService;
        private readonly IAlarmMdcsService _alarmMdcsService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly ITaskInstanceService _taskInstanceService;
        private readonly IStorageService _storageService;
        private readonly IStorageTagService _storageTagService;
        private readonly INodeService _nodeService;
        private readonly IEdgeService _edgeService;
        private readonly IMapService _mapService;
        private readonly IDistributedCache _distributedCache;
        private readonly List<DictItemDto>? _dictItemDtos;
        private readonly ConfigDataDto? _configDataDto;
        public List<NodeDto> nodeDtos;
        public List<EdgeDto> edgeDtos;
        public bool IsMdcs { get; set; }
        public OffsetParameter? Offset { get; set; } = null;

        public RuntimeHub(
            ILogger<RuntimeHub> logger,
            ICarService carService,
            IAlarmService alarmService,
            IAlarmMdcsService alarmMdcsService,
            ITaskRecordService taskRecordService,
            ITaskInstanceService taskInstanceService,
            IStorageService storageService,
            IStorageTagService storageTagService,
            INodeService nodeService,
            IEdgeService edgeService,
            IMapService mapService,
            IDistributedCache distributedCache)
        {
            _logger = logger;
            _carService = carService;
            _alarmService = alarmService;
            _alarmMdcsService = alarmMdcsService;
            _taskRecordService = taskRecordService;
            _taskInstanceService = taskInstanceService;
            _storageService = storageService;
            _storageTagService = storageTagService;
            _nodeService = nodeService;
            _edgeService = edgeService;
            _mapService = mapService;
            _distributedCache = distributedCache;
            _dictItemDtos = _distributedCache.Get<List<DictItemDto>>(CacheKey.Setting.DictItem);
            _configDataDto = _distributedCache.Get<ConfigDataDto>(CacheKey.Setting.ConfigData);
            nodeDtos = _nodeService.Set().ToList();
            edgeDtos = _edgeService.Set().ToList();
            IsMdcs = _carService.Set().Where(e => e.IsEnable && e.Type != null && !e.Type.Equals("Fairyland.Pc")).ToList().Count > 0 ? false : true;
            InitOffsetParameter();
        }

        public override Task OnConnectedAsync()
        {
            InitOffsetParameter();//防止修改地图后没有加载页面
            return base.OnConnectedAsync();
        }

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}

        public void InitOffsetParameter()
        {
            var mapDto = _mapService.FirstOrDefault(e => e.IsEnable);
            if (mapDto != null)
            {
                var mapExtendDto = _mapService.GetExtends(mapDto).FirstOrDefault(e => e.Key == "Abs");
                if (mapExtendDto != null && mapExtendDto.Value != null)
                {
                    Offset = mapExtendDto.Value.JsonTo<OffsetParameter>();
                }
            }
        }
        public async Task Update()
        {
            Thread.Sleep(50);

            var _cars = new List<Car>();
            var _maps = new List<Map>();
            var _alarms = new List<Alarm>();
            var _status = new StatusStatistics();
            var _tasks = new List<CarTask>();
            var _storages = new List<Storage>();
            var _nodes = new List<Node>();
            var _lowBatteryCars = new List<Car>();
            var minBattery = 30;
            if (_configDataDto?.CarMinBattery is not null)
            {
                minBattery = int.Parse(_configDataDto.CarMinBattery);
            }

            try
            {
                var cars = _carService.GetRuntime(e => e.IsEnable == true).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToList();

                //新增统计车辆状态
                _status.OffLine = cars.Where(e => e.IsOnline == false || e.IsNormal == false).Count();
                _status.Idle = cars.Where(e => e.IsOnline == true && e.IsNormal == true && CarConstExtend.State.Idle.Contains(e.CurrState)).Count();
                _status.Charging = cars.Where(e => e.IsOnline == true && e.IsNormal == true && e.CurrState == CarConstExtend.State.Charging).Count();
                _status.Tasking = cars.Where(e => e.IsOnline == true && e.IsNormal == true && CarConstExtend.State.Executing.Contains(e.CurrState)).Count();
                _status.Faulting = cars.Where(e => e.IsOnline == true && CarConstExtend.State.Malfunction.Contains(e.CurrState) && e.IsNormal == true).Count();

                foreach (var car in cars)
                {
                    var _car = new Hubs.Monitor.Models.Car
                    {
                        Code = car.Code,
                        Name = car.Name,
                        CurrNodeCode = car.CurrNode?.Code,
                        Charge = car.Battery.ToString()
                    };
                    FASS.Web.Api.Models.Pc.CarState? carState = null;
                    _car.State = "离线";
                    if (car.Type == "Fairyland.Pc")
                    {
                        if (car.CarState != null)
                        {
                            car.CarState.TryJsonTo<FASS.Web.Api.Models.Pc.CarState>(out carState);
                            _car.StopAccept = carState?.StopAccept == null ? false : carState.StopAccept;
                        }
                    }
                    if (CarConstExtend.State.Idle.Contains(car.CurrState) && car.IsOnline == true && car.IsNormal == true)
                        _car.State = "空闲";
                    if (CarConstExtend.State.Executing.Contains(car.CurrState) && car.IsOnline == true && car.IsNormal == true)
                    {
                        if (car.Type == "Fairyland.Pc")
                        {
                            _car.State = carState?.Load == 0 ? "任务中[空载]" : "任务中[满载]";
                        }
                        else
                        {
                            _car.State = "任务中";
                        }
                    }
                    if (CarConstExtend.State.Charging == car.CurrState && car.IsOnline == true && car.IsNormal == true)
                        _car.State = "充电中";
                    if (CarConstExtend.State.Malfunction.Contains(car.CurrState) && car.IsOnline == true && car.IsNormal == true)
                        _car.State = "异常";
                    if (_car.State == "空闲" || _car.State.Contains("任务中"))
                    {
                        if (carState?.Warns is not null)
                        {
                            if (carState.Warns.Count > 0)
                            {
                                _car.State = "告警";//传递告警
                            }
                        }
                        if (car.Battery < minBattery)
                        {
                            _car.State = "告警";//迷毂判定的低电量
                        }
                    }
                    _cars.Add(_car);

                    var _map = new Map();
                    _map.Type = car.CarType.Code;
                    _map.Code.Text = car.Code;
                    _map.Name.Text = car.Name;
                    _map.Base.Point.X = car.X;
                    _map.Base.Point.Y = car.Y;
                    _map.Base.Size.W = car.Length;
                    _map.Base.Size.H = car.Width;
                    _map.Base.Rotate = car.Theta;
                    if (car.Type == "Fairyland.Pc")
                    {
                        //偏转坐标
                        if (Offset is not null)
                        {
                            double normalNode = Offset.NormNodeW;
                            _map.Base.Point.X = car.X + Offset.AbsMinX + normalNode;
                            _map.Base.Point.Y = car.Y * -1 + Offset.AbsMinY + normalNode;
                        }
                        else
                        {
                            _map.Base.Point.X = car.X;
                            _map.Base.Point.Y = car.Y;
                        }
                        var carNodes = new List<CarNode>();
                        var carEdges = new List<CarEdge>();
                        //if (!car.CarState.TryJsonTo<FASS.Web.Api.Models.Pc.CarState>(out var carState) || string.IsNullOrEmpty(car.CarState))
                        //{
                        //    _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 车辆状态错误");
                        //    continue;
                        //}
                        if (carState == null || string.IsNullOrEmpty(car.CarState))
                        {
                            _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 车辆状态错误");
                            continue;
                        }
                        //当前手自动状态
                        _map.Base.Status.ManualMode = (carState.ManualMode == "1" ? "手动" : "自动");
                        //当前锁点
                        var currNodes = carState.HoldingLocks.ToList();
                        if (currNodes.Count == 0)
                        {
                            _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 车辆未上报当前站点");
                            continue;
                        }
                        if (currNodes.Count > 1)
                        {
                            foreach (var item in currNodes)
                            {
                                var node = nodeDtos.Where(e => e.Code == item.ToString()).FirstOrDefault();
                                if (node != null)
                                {
                                    carNodes.Add(new Hubs.Monitor.Models.CarNode()
                                    {
                                        Id = node.Id.Replace("_reverse", ""),
                                        Type = "Curr",
                                        Process = 1
                                    });
                                }
                            }
                        }
                        //待锁点
                        var nextNodes = carState.PendingLocks.ToList();
                        foreach (var item in nextNodes)
                        {
                            var node = nodeDtos.Where(e => e.Code == item.ToString()).FirstOrDefault();
                            if (node != null)
                            {
                                carNodes.Add(new Hubs.Monitor.Models.CarNode()
                                {
                                    Id = node.Id.Replace("_reverse", ""),
                                    Type = "Next",
                                    Process = 0.5
                                });
                            }
                        }
                        _map.Base.CarNodes = carNodes;
                        //根据当前锁点 + 待锁点 拼接路线
                        if (carNodes.Count > 1)
                        {
                            for (int i = 0; i < carNodes.Count - 1; i++)
                            {
                                var edge = edgeDtos.Where(e => e.StartNodeId == carNodes[i].Id
                                    && e.EndNodeId == carNodes[i + 1].Id).FirstOrDefault();
                                if (edge != null)
                                {
                                    var type = (carNodes[i].Type == carNodes[i + 1].Type
                                        && carNodes[i].Type == "Next") ? "Next" : "Curr";
                                    carEdges.Add(new Hubs.Monitor.Models.CarEdge()
                                    {
                                        Id = edge.Id.Replace("_reverse", ""),
                                        Type = type,
                                        Process = (type == "Next") ? 0.5 : 0
                                    });
                                }
                            }
                        }
                        _map.Base.CarEdges = carEdges;
                    }
                    else
                    {
                        //_map.Base.CarEdges = car.CarEdges.Select(e1 => new Hubs.Monitor.Models.CarEdge() { Id = e1.EdgeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process });
                        //_map.Base.CarNodes = car.CarNodes.Select(e1 => new Hubs.Monitor.Models.CarNode() { Id = e1.NodeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process });
                        var carEdges = car.CarEdges.Select(e1 => new CarEdge() { Id = e1.EdgeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process }).ToList();
                        var carNodes = car.CarNodes.Select(e1 => new CarNode() { Id = e1.NodeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process }).ToList();
                        if (car.CarEdges.All(e => e.Type == CarEdgeConst.Type.Prev) || car.CarNodes.All(e => e.Type == CarNodeConst.Type.Prev))
                        {
                            carEdges.Clear();
                            carNodes.Clear();
                        }
                        _map.Base.CarEdges = carEdges;
                        _map.Base.CarNodes = carNodes;
                    }
                    _maps.Add(_map);
                }

                var alarms = _alarmMdcsService.Set().Where(e => !e.IsFinish).OrderByDescending(e => e.CreateAt).Take(20).ToList();

                foreach (var alarm in alarms)
                {
                    var _alarm = new Hubs.Monitor.Models.Alarm();
                    _alarm.Code = alarm.CarCode;//车辆编码
                    _alarm.Name = alarm.CarName;//车辆编码
                    _alarm.StartTime = alarm.StartTime;//报警开始时间
                    _alarm.State = alarm.Code;//报警编码
                    _alarm.Message = alarm.Name;//报警描述
                    _alarms.Add(_alarm);
                    #region 关联报警记录
                    //var alarmCar = cars.Where(e => e.Code == alarm.CarCode).FirstOrDefault();
                    //if (alarmCar is not null && alarmCar.Type == "Fairyland.Pc" && alarmCar.CarState is not null)
                    //{
                    //    alarmCar.CarState.TryJsonTo<FASS.Web.Api.Models.Pc.CarState>(out var alarmCarState);
                    //    if (alarmCarState is not null && alarmCarState.Alarms.Where(e => e.Code == alarm.Code).Count() > 0)
                    //    {
                    //        _alarms.Add(_alarm);
                    //    }
                    //}
                    //else
                    //{
                    //    _alarms.Add(_alarm);
                    //}
                    #endregion
                }

                if (IsMdcs)
                {
                    var tasks = _taskRecordService.Set().OrderByDescending(e => e.CreateAt).Take(20).ToList();
                    foreach (var task in tasks)
                    {
                        var _task = new Hubs.Monitor.Models.CarTask
                        {
                            Code = task.Code ?? "",
                            State = TaskRecordConst.State.GetStatusDescription(task.State ?? ""),
                            CarCode = cars.Where(e => e.Id == task.CarId).FirstOrDefault()?.Code,
                            CarName = cars.Where(e => e.Id == task.CarId).FirstOrDefault()?.Name
                        };
                        _tasks.Add(_task);
                    }
                }
                else
                {
                    var taskInstances = _taskInstanceService.Set().OrderByDescending(e => e.CreateAt).Take(20).ToList();
                    foreach (var task in taskInstances)
                    {
                        var _task = new Hubs.Monitor.Models.CarTask
                        {
                            Code = task.Code,
                            State = TaskRecordConst.State.GetStatusDescription(task.State),
                            CarCode = cars.Where(e => e.Id == task.CarId).FirstOrDefault()?.Code,
                            CarName = cars.Where(e => e.Id == task.CarId).FirstOrDefault()?.Name
                        };
                        _tasks.Add(_task);
                    }
                }

                var storageTagDtos = _storageTagService.Set().ToList();
                var storageDtos = _storageService.Set().Where(e => e.IsEnable).ToList();
                storageDtos.ForEach(e =>
                {
                    if (storageTagDtos.Where(p => p.StorageId == e.Id).Count() > 0 && e.State == StorageConst.State.NoneContainer)
                    {
                        e.State = StorageConst.State.HasTag;
                    }
                });
                _storages = storageDtos.Select(e => new Storage { Code = e.Code, NodeCode = e.NodeCode, State = e.State, IsLock = e.IsLock }).ToList();

                //禁用站点
                var lockNodeDtos = _nodeService.Set().AsNoTracking().Where(e => e.IsLock).ToList();
                _nodes = lockNodeDtos.Select(e => new Node
                {
                    Id = e.Id,
                    Code = e.Code,
                    Type = e.Type
                }).ToList();

                //低电量车辆
                _lowBatteryCars = _cars.Where(e => double.Parse(e.Charge ?? "0") < minBattery).ToList();
                var data = new { car = _cars, map = _maps, alarm = _alarms, status = _status, task = _tasks, storage = _storages, lockNode = _nodes, lowBatteryCars = _lowBatteryCars };

                await Clients.Caller.SendAsync("Update", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}