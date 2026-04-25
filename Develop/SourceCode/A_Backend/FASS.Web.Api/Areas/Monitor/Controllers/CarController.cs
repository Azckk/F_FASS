using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Data.Services.Data.Interfaces;
using FASS.Service.Consts.Data;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Models.Pc;
using FASS.Web.Api.Models.Plc;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;


namespace FASS.Web.Api.Areas.Monitor.Controllers
{
    [Route("api/v1/Monitor/[controller]/[action]")]
    [Tags("监控管理-车辆监控")]
    public class CarController : BaseController
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarService _carService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly ICarTrafficService _carTrafficService;
        private readonly AppSettings _appSettings;

        public CarController(
            ILogger<CarController> logger,
            ICarService carService,
            ITaskRecordService taskRecordService,
            ICarTrafficService carTrafficService,
            AppSettings appSettings
        )
        {
            _logger = logger;
            _carService = carService;
            _taskRecordService = taskRecordService;
            _carTrafficService = carTrafficService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _carService.ToPageAsync(param);
            if (page == null)
            {
                return BadRequest("请求参数有误");
            }
            var carlist = page.Data;
            var carMonitorList = new List<CarMonitor>();
            var taskRecordDtos = await _taskRecordService
                .Set()
                .Where(e => TaskRecordConst.State.carTaskType.Contains(e.State))
                .ToListAsync();
            var carTrafficDtos = await _carTrafficService.Set().ToListAsync();
            var cars = await _carService.Set().ToListAsync();
            foreach (var car in carlist)
            {
                var carMonitor = new CarMonitor()
                {
                    Code = car.Code,
                    Name = car.Name,
                    Type = car.Type,
                    Battery = car.Battery,
                    IpAddress = car.IpAddress,
                    IsOnline = car.IsOnline,
                    CurrState = car.CurrState,
                    UpdateAt = car.UpdateAt
                };
                if (car.IsOnline)
                {
                    if (car.UpdateAt < DateTime.Now.AddSeconds(-5))
                    {
                        carMonitor.IsOnline = false;
                    }
                }

                carMonitor.CurrState = "未设置";
                if (CarConstExtend.State.Idle.Contains(car.CurrState) && car.IsOnline == true)
                    carMonitor.CurrState = "空闲";
                if (CarConstExtend.State.Executing.Contains(car.CurrState) && car.IsOnline == true)
                    carMonitor.CurrState = "任务中";
                if (CarConstExtend.State.Charging == car.CurrState && car.IsOnline == true)
                    carMonitor.CurrState = "充电中";
                if (CarConstExtend.State.Malfunction.Contains(car.CurrState) || !car.IsNormal)
                    carMonitor.CurrState = "异常";

                var carTask = taskRecordDtos
                    .Where(e => e.CarId == car.Id)
                    .OrderBy(e => e.CreateAt)
                    .FirstOrDefault();
                if (carTask != null)
                {
                    carMonitor.CurrentTaskId = carTask.Id;
                }
                if (car.Type == "Fairyland.Pc")
                {
                    if (car.CarState != null && car.CarState.TryJsonTo<CarState>(out var carState) && carState != null)
                    {
                        carMonitor.IsAlarm = carState.Alarms.Count > 0 ? true : false;
                        carMonitor.AlarmStatu = carMonitor.IsAlarm ? string.Join(",", carState.Alarms.Select(e => e.Name)) : "/";
                        carMonitor.IsBlockedBy = carState.BlockedBy.Count > 0 ? true : false;
                        carMonitor.StopAccept = carState.StopAccept;
                        if (carState.BlockedBy.Count > 0)
                        {
                            var blockedCars = carState.BlockedBy.Select(e => e.CarCode).ToArray();
                            var names = cars.Where(e => blockedCars.Contains(e.Code)).Select(e => e.Name);
                            carMonitor.TrafficMessage = $"{string.Join(",", names)}交管";
                        }
                        else
                        {
                            if (carState.TrafficMessage is not null)
                            {
                                if (carState.TrafficMessage.Contains("PtlInterlockMission"))
                                {
                                    carMonitor.TrafficMessage = "ptl交管";
                                    carMonitor.IsBlockedBy = true;
                                }
                                else if (carState.TrafficMessage.Contains("NSInterMission"))
                                {
                                    carMonitor.TrafficMessage = "机构交管";
                                    carMonitor.IsBlockedBy = true;
                                }
                                else
                                {
                                    carMonitor.TrafficMessage = "";//无交管信息，显示空
                                }
                            }
                            else
                            {
                                carMonitor.TrafficMessage = "";//无交管信息，显示空
                            }
                        }
                        if (carState.HoldingLocks != null)
                        {
                            carMonitor.CurrNodeCode = carState.HoldingLocks.StringJoin(",");
                        }
                        if (carState.PendingLocks != null)
                        {
                            carMonitor.NextNodeCode = carState.PendingLocks.StringJoin(",");
                        }

                    }
                    var taskRecordDto = await _taskRecordService
                        .Set()
                        .Where(e => TaskRecordConst.State.carTaskType.Contains(e.State) && e.Id == car.Id)
                        .FirstOrDefaultAsync();
                    carMonitor.CurrentTaskId = taskRecordDto?.Id;
                }
                else
                {
                    if (car.CarState != null && car.CarState.TryJsonTo<ReceiveStateMessage>(out var carState) && carState != null)
                    {
                        carMonitor.IsAlarm = carState.Alarm > 0 ? true : false;
                    }
                    carMonitor.CurrNodeCode = car.CurrNodeCode;
                    carMonitor.NextNodeCode = car.NextNodeCode;
                }
                carMonitorList.Add(carMonitor);
            }
            var data = new
            {
                rows = carMonitorList,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _carService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarInfo([FromQuery] string keyValue)
        {
            try
            {
                //传入车辆id或车辆code都可行
                var cars = await _carService.Set().ToListAsync();
                var car = cars.FirstOrDefault(e => e.Code == keyValue || e.Id == keyValue);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{keyValue}]");
                }
                var carMonitor = new CarMonitor()
                {
                    Code = car.Code,
                    Name = car.Name,
                    Type = car.Type, //协议类型
                    CarTypeName = car.CarTypeName,
                    Speed = car.Speed,
                    CurrState = car.CurrState,
                    X = car.X,
                    Y = car.Y,
                    Battery = car.Battery,
                    IpAddress = car.IpAddress,
                    Port = car.Port,
                    IsOnline = car.IsOnline,
                };
                if (car.Type == "Fairyland.Pc")
                {
                    if (car.CarState != null && car.CarState.TryJsonTo<CarState>(out var carState) && carState != null)
                    {
                        carMonitor.IsAlarm = carState.Alarms.Count > 0 ? true : false;
                        carMonitor.AlarmStatu = carMonitor.IsAlarm ? string.Join(",", carState.Alarms.Select(e => e.Name)) : "/";
                        carMonitor.IsBlockedBy = carState.BlockedBy.Count > 0 ? true : false;
                        carMonitor.StopAccept = carState.StopAccept;
                        carMonitor.ManualMode = carState.ManualMode == "1" ? "手动" : "自动";
                        carMonitor.InterLockStatu = carState.InterLockStatu;
                        carMonitor.Tags = carState.Tags;
                        if (carState.BlockedBy.Count > 0)
                        {
                            var blockedCars = carState.BlockedBy.Select(e => e.CarCode).ToArray();
                            var names = cars.Where(e => blockedCars.Contains(e.Code)).Select(e => e.Name);
                            carMonitor.TrafficMessage = $"{string.Join(",", names)}交管";
                        }
                        else
                        {
                            if (carState.TrafficMessage is not null)
                            {
                                if (carState.TrafficMessage.Contains("PtlInterlockMission"))
                                {
                                    carMonitor.TrafficMessage = "ptl交管";
                                    carMonitor.IsBlockedBy = true;
                                }
                                else if (carState.TrafficMessage.Contains("NSInterMission"))
                                {
                                    carMonitor.TrafficMessage = "机构交管";
                                    carMonitor.IsBlockedBy = true;
                                }
                                else
                                {
                                    carMonitor.TrafficMessage = "";//无交管信息，显示空
                                }
                            }
                            else
                            {
                                carMonitor.TrafficMessage = "";//无交管信息，显示空
                            }
                        }
                        if (carState.HoldingLocks != null)
                        {
                            carMonitor.CurrNodeCode = carState.HoldingLocks.StringJoin(",");
                        }
                        if (carState.PendingLocks != null)
                        {
                            carMonitor.NextNodeCode = carState.PendingLocks.StringJoin(",");
                        }
                        /*    var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                                $"http://{car.IpAddress}:9776/car/getCarCpu"
                            );
                            if (resp!=null&&resp.Code == 200)
                            {
                                resp.Data.ToString().TryJsonTo<HardwareInfo>(out var info);
                                carMonitor.Cpu= info.Cpu;
                                carMonitor.Gpu=info.Gpu;
                                carMonitor.Tmp= info.Tmp;
                                carMonitor.Memory= info.Memory;
                                carMonitor.NetW= info.NetW;
                            }
                            else
                            {
                                return BadRequest($"车辆 [{car.Code}] 获取硬件信息失败 =>{resp.Message}");
                            }*/
                    }
                    var taskRecordDto = await _taskRecordService
                        .Set()
                        .Where(e => TaskRecordConst.State.carTaskType.Contains(e.State) && e.Id == car.Id)
                        .FirstOrDefaultAsync();
                    carMonitor.CurrentTaskId = taskRecordDto?.Id;
                }
                else
                {
                    if (car.CarState != null && car.CarState.TryJsonTo<ReceiveStateMessage>(out var carState) && carState != null)
                    {
                        carMonitor.IsAlarm = carState.Alarm > 0 ? true : false;
                    }
                    carMonitor.CurrNodeCode = car.CurrNodeCode;
                    carMonitor.NextNodeCode = car.NextNodeCode;
                }
                return Ok(carMonitor);
            }
            catch (Exception ex)
            {
                return BadRequest($"获取车辆 [{keyValue}]信息失败 =>{ex}");
            }
        }


        /// <summary>
        /// 获取小车工控机硬件信息
        /// </summary>
        /// <param name="Param">小车编号</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> GetPcHardware([FromQuery] string carCode)
        {
            try
            {

                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"http://{car.IpAddress}:9776/getCarCpu"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 获取硬件信息失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]获取硬件信息失败 =>{ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCarMethods([FromQuery] string carCode)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆方法失败 [{carCode}]");
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car_reflection/get_methods/{carCode}"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest($"车辆 [{carCode}] 获取车辆方法失败 =>{resp.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"车辆 [{carCode}]获取车辆方法失败 =>{ex}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ExecuteCarMethod([FromQuery] string carCode, [FromQuery] string method, [FromQuery] string? param)
        {
            try
            {
                var car = await _carService.FirstOrDefaultAsync(e => e.Code == carCode);
                if (car == null)
                {
                    return BadRequest($"获取车辆失败 [{carCode}]");
                }
                string paramString = string.Empty;
                var paramters = param?.Replace("undefined", "")?.JsonTo<List<Parameter>>();
                if (paramters is not null)
                {
                    foreach (var item in paramters)
                    {
                        paramString += $"&{item.Key}={item.Value}";
                    }
                    paramString = $"?{paramString.TrimStart("&")}";
                }
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/car_reflection/execute/{carCode}/{method}{paramString}"
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
                return BadRequest($"执行 car method失败 carId:[{carCode}],method:[{method}],param:{param} {ex.Message}");
            }
        }

    }
}
