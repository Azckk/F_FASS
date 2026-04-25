using Common.AspNetCore.Extensions;
using Common.Frame.Dtos.Frame;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Services.Base.Interfaces;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Record.Interfaces;
using FASS.Service.Consts.Data;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.RecordExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Hubs.Monitor.Models;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Monitor.Controllers
{
    [Route("api/v1/Monitor/[controller]/[action]")]
    [Tags("监控管理-运行监控")]
    public class RuntimeController : BaseController
    {
        private readonly ILogger<RuntimeController> _logger;
        private readonly IMapService _mapService;
        private readonly ICarService _carService;
        private readonly IAlarmService _alarmService;
        private readonly IAlarmMdcsService _alarmMdcsService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly IStorageService _storageService;
        private readonly IStorageTagService _storageTagService;
        private readonly INodeService _nodeService;
        private readonly IDistributedCache _distributedCache;
        private readonly List<DictItemDto>? _dictItemDtos;
        private readonly AppSettings _appSettings;
        public OffsetParameter Offset { get; set; } = null!;

        public RuntimeController(
            ILogger<RuntimeController> logger,
            IMapService mapService,
            ICarService carService,
            IAlarmService alarmService,
            IAlarmMdcsService alarmMdcsService,
            ITaskRecordService taskRecordService,
            IStorageService storageService,
            IStorageTagService storageTagService,
            INodeService nodeService,
            IDistributedCache distributedCache,
            AppSettings appSettings)
        {
            _logger = logger;
            _mapService = mapService;
            _carService = carService;
            _alarmService = alarmService;
            _alarmMdcsService = alarmMdcsService;
            _taskRecordService = taskRecordService;
            _storageService = storageService;
            _storageTagService = storageTagService;
            _nodeService = nodeService;
            _distributedCache = distributedCache;
            _dictItemDtos = _distributedCache.Get<List<DictItemDto>>(CacheKey.Setting.DictItem);
            _appSettings = appSettings;
        }

        //public override IActionResult Index()
        //{
        //    ViewBag.ServiceApi = "/Monitor/RuntimeHub";
        //    return base.Index();
        //}

        [HttpGet]
        public async Task<IActionResult> GetMap()
        {
            var data = new
            {
                map = await _mapService.Set().FirstOrDefaultAsync()
            };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult GetUpdate()
        //{
        //    var _cars = new List<Hubs.Monitor.Models.Car>();
        //    var _maps = new List<Hubs.Monitor.Models.Map>();
        //    var _alarms = new List<Hubs.Monitor.Models.Alarm>();

        //    var cars = _carService.GetRuntime(e => e.IsEnable == true).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToList();

        //    foreach (var car in cars)
        //    {
        //        var _car = new Hubs.Monitor.Models.Car();
        //        _car.Code = car.Code;
        //        _car.CurrNodeCode = car.CurrNode?.Code;
        //        _car.Charge = car.Battery.ToString();
        //        _car.State = _dictItemDtos.Where(e1 => e1.Dict.Code == "CarState").FirstOrDefault(e1 => e1.Code == car.CurrState)?.Name;
        //        _car.State = car.IsNormal ? _car.State : "故障";
        //        _car.State = car.IsOnline ? _car.State : "离线";
        //        _cars.Add(_car);

        //        var _map = new Hubs.Monitor.Models.Map();
        //        _map.Type = car.CarType.Code;
        //        _map.Code.Text = car.Code;
        //        _map.Base.Point.X = car.X;
        //        _map.Base.Point.Y = car.Y;
        //        _map.Base.Size.W = car.Length;
        //        _map.Base.Size.H = car.Width;
        //        _map.Base.Rotate = car.Theta;
        //        _map.Base.CarEdges = car.CarEdges.Select(e1 => new Hubs.Monitor.Models.CarEdge() { Id = e1.EdgeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process });
        //        _map.Base.CarNodes = car.CarNodes.Select(e1 => new Hubs.Monitor.Models.CarNode() { Id = e1.NodeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process });
        //        _maps.Add(_map);
        //    }

        //    var alarms = _alarmService.Set().Where(e => e.Type == AlarmConst.Type.CarAlarm).OrderByDescending(e => e.CreateAt).Take(20).ToList();

        //    foreach (var alarm in alarms)
        //    {
        //        var _alarm = new Hubs.Monitor.Models.Alarm();
        //        _alarm.Level = _dictItemDtos.Where(e1 => e1.Dict.Code == "AlarmLevel").FirstOrDefault(e1 => e1.Code == alarm.Level)?.Name;
        //        _alarm.Type = alarm.Type;
        //        _alarm.Code = alarm.Code;
        //        _alarm.Message = alarm.Message;
        //        _alarms.Add(_alarm);
        //    }

        //    var data = new { car = _cars, map = _maps, alarm = _alarms };

        //    return Ok(data);
        //}

        [HttpGet]
        public IActionResult GetSimpleMap()
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/map/getMap"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取simple地图失败：{ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetPointCloudMap()
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/map/getLidarMap"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取simple点云地图失败：{ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStorageListToSelect()
        {
            var data = await _storageService.Set().Where(e => e.IsEnable).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        public IActionResult GetUpdate()
        {
            var _cars = new List<Hubs.Monitor.Models.Car>();
            var _maps = new List<Hubs.Monitor.Models.Map>();
            var _alarms = new List<Hubs.Monitor.Models.Alarm>();
            var _status = new Hubs.Monitor.Models.StatusStatistics();
            var _tasks = new List<Hubs.Monitor.Models.CarTask>();
            var _storages = new List<Hubs.Monitor.Models.Storage>();
            var _nodes = new List<Hubs.Monitor.Models.Node>();

            var cars = _carService.GetRuntime(e => e.IsEnable == true).OrderBy(e => e.Code.Length).ThenBy(e => e.Code).ToList();

            //新增统计车辆状态
            _status.OffLine = cars.Where(e => e.IsOnline == false).Count();
            _status.Idle = cars.Where(e => e.IsOnline == true && CarConstExtend.State.Idle.Contains(e.CurrState)).Count();
            _status.Charging = cars.Where(e => e.IsOnline == true && e.CurrState == CarConstExtend.State.Charging).Count();
            _status.Tasking = cars.Where(e => e.IsOnline == true && CarConstExtend.State.Executing.Contains(e.CurrState)).Count();
            _status.Faulting = cars.Where(e => e.IsOnline == true && CarConstExtend.State.Malfunction.Contains(e.CurrState)).Count();

            //获取地图偏转参数
            var mapDto = _mapService.FirstOrDefault(e => e.IsEnable);
            if (mapDto != null)
            {
                var mapExtendDto = _mapService.GetExtends(mapDto).FirstOrDefault(e => e.Key == "Abs");
                if (mapExtendDto != null)
                {
                    var offsetParameter = (mapExtendDto.Value ?? "").JsonTo<OffsetParameter>();
                    if (offsetParameter != null)
                        Offset = offsetParameter;
                }
            }

            foreach (var car in cars)
            {
                var _car = new Hubs.Monitor.Models.Car
                {
                    Code = car.Code,
                    Name = car.Name,
                    CurrNodeCode = car.CurrNode?.Code,
                    Charge = car.Battery.ToString(),
                    State = "离线"
                };
                if (car.Type == "Fairyland.Pc")
                {
                    if (car.CarState != null && car.CarState.TryJsonTo<CarState>(out var carState) && carState != null)
                    {
                        _car.StopAccept = carState.StopAccept;
                    }
                }
                if (CarConstExtend.State.Idle.Contains(car.CurrState) && car.IsOnline == true)
                    _car.State = "空闲";
                if (CarConstExtend.State.Executing.Contains(car.CurrState) && car.IsOnline == true)
                    _car.State = "任务中";
                if (CarConstExtend.State.Charging == car.CurrState && car.IsOnline == true)
                    _car.State = "充电中";
                if (CarConstExtend.State.Malfunction.Contains(car.CurrState) && car.IsOnline == true)
                    _car.State = "异常";
                _cars.Add(_car);

                var _map = new Hubs.Monitor.Models.Map();
                _map.Type = car.CarType.Code;
                _map.Code.Text = car.Code;
                _map.Name.Text = car.Name;
                if (Offset != null)
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
                _map.Base.Size.W = car.Length;
                _map.Base.Size.H = car.Width;
                _map.Base.Rotate = car.Theta;
                _map.Base.CarEdges = car.CarEdges.Select(e1 => new Hubs.Monitor.Models.CarEdge() { Id = e1.EdgeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process });
                _map.Base.CarNodes = car.CarNodes.Select(e1 => new Hubs.Monitor.Models.CarNode() { Id = e1.NodeId.Replace("_reverse", ""), Type = e1.Type, Process = e1.Process });
                _maps.Add(_map);
            }

            var alarms = _alarmMdcsService.Set().Where(e => !e.IsFinish).OrderByDescending(e => e.CreateAt).Take(20).ToList();

            foreach (var alarm in alarms)
            {
                var _alarm = new Hubs.Monitor.Models.Alarm
                {

                    Code = alarm.CarCode,//车辆编码
                    Name = alarm.Name,//车辆名称
                    StartTime = alarm.StartTime,//报警开始时间
                    State = alarm.Code,//报警编码
                    Message = alarm.Name//报警描述
                };
                _alarms.Add(_alarm);
            }

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

            var storageTagDtos = _storageTagService.Set().ToList();
            var storageDtos = _storageService.Set().Where(e => e.IsEnable).ToList();
            storageDtos.ForEach(e =>
            {
                if (storageTagDtos.Where(p => p.StorageId == e.Id).Count() > 0 && e.State == StorageConst.State.NoneContainer)
                {
                    e.State = StorageConst.State.HasTag;
                }
            });
            _storages = storageDtos.Select(e => new Hubs.Monitor.Models.Storage { Code = e.Code, NodeCode = e.NodeCode, State = e.State }).ToList();

            //禁用站点
            var lockNodeDtos = _nodeService.Set().Where(e => e.IsLock).ToList();
            _nodes = lockNodeDtos.Select(e => new Hubs.Monitor.Models.Node
            {
                Id = e.Id,
                Code = e.Code
            }).ToList();

            var data = new { car = _cars, map = _maps, alarm = _alarms, status = _status, task = _tasks, storage = _storages, lockNode = _nodes };

            return Ok(data);
        }
    }
}