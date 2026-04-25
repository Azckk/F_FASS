using Common.AspNetCore.Extensions;
using Common.NETCore.Extensions;
using FASS.Boot.Services.Cars;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Instant;
using FASS.Data.Models.Data;
using FASS.Extend.Car.Fairyland.Pc.Models.Response;
using FASS.Scheduler.Models;
using FASS.Scheduler.Services.Cars.Fairyland.Pc.Extensions;
using FASS.Service.Models.RecordExtend;
using FASS.Service.Services.RecordExtend.Interfaces;
using CarActionConst = FASS.Service.Consts.Data.CarActionConst;

namespace FASS.Scheduler.Services.Cars.Fairyland.Pc
{
    public class CarRequestService : ICarRequestService
    {
        private readonly ILogger<CarRequestService> _logger;
        private readonly AppSettings _appSettings;
        private CarService _carService = null!;
        private CarSessionService _carSessionService = null!;
        private readonly ITrafficService _trafficService;
        private string _carTrafficInfo = string.Empty;//存放车辆交管信息
        private bool _firstLoad = true;//首次加载

        public CarRequestService(
            ILogger<CarRequestService> logger,
            AppSettings appSettings,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _appSettings = appSettings;
            _trafficService = serviceProvider.GetScopeService<ITrafficService>();
        }

        public void Initialize(CarService carService)
        {
            _carService = carService;
            _carSessionService = (CarSessionService)_carService.CarSessionService;

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var last = DateTime.Now;
                while (true)
                {
                    if ((DateTime.Now - last).TotalMilliseconds >= 3000)
                    {
                        last = DateTime.Now;
                        ScanTrafficRecord(_carService.Car);//交管记录判定
                    }
                    Thread.Sleep(100);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public bool TryExecute(Car car, out Car result)
        {
            result = car;
            try
            {
                TryGetCarInstantAction(car, out result);
                TryGetTaskInstance(car, out result);
                Thread.Sleep(_appSettings.Scheduler.StateDueTime);
                _carSessionService.SendState();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public bool TryGetCarInstantAction(Car car, out Car result)
        {
            result = car;
            if (car.CurrCarInstantAction is null)
            {
                return false;
            }
            if (CarInstantActionConst.State.Stop.Contains(car.CurrCarInstantAction.State))
            {
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Start && car.CurrState != CarConst.State.Running)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendAction(car.ToCarAction());
                car.NextState = CarConst.State.Running;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Stop && car.CurrState != CarConst.State.Stopping)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendAction(car.ToCarAction());
                car.NextState = CarConst.State.Stopping;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.EmergencyStop && car.CurrState != CarConst.State.EmergencyStop)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                var carInstantAction = car.ToCarAction();
                _carSessionService.SendAction(carInstantAction);
                car.NextState = CarConst.State.EmergencyStop;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Reset && car.CurrState != CarConst.State.Running)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                var carInstantAction = car.ToCarAction();
                _carSessionService.SendAction(carInstantAction);
                car.NextState = CarConst.State.Running;
                return false;
            }
            if (!car.CurrCarInstantAction.CarInstantParameters.Any())
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 未设置参数");
                return false;
            }
            if (car.CarState is null || !car.CarState.TryJsonTo<CarState>(out var carState))
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 车辆状态错误");
                return false;
            }
            var defaultValue = car.CurrCarInstantAction.CarInstantParameters.FirstOrDefault(e => e.Key.Equals("Default", StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 未设置参数 [Default]");
                return false;
            }
            var carAction = carState?.Actions.FirstOrDefault(e => e.Code == car.CurrCarInstantAction.Id);
            if (carAction is null || carAction.State != CarInstantActionConst.State.Completed)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendAction(car.ToCarAction());
                return false;
            }
            return true;
        }

        public bool TryGetTaskInstance(Car car, out Car result)
        {
            result = car;
            if (car.CurrTaskInstance is null)
            {
                return false;
            }
            if (TaskInstanceConst.State.Stop.Contains(car.CurrTaskInstance.State))
            {
                return false;
            }
            if (car.CurrTaskInstance.State == TaskInstanceConst.State.Paused)
            {
                return false;
            }
            if (car.CurrTaskInstance.State == TaskInstanceConst.State.Resumed)
            {
                return false;
            }
            //if (car.CurrTaskInstance.State == TaskInstanceConst.State.Canceling)
            //{
            //    return false;
            //}
            //if (car.CurrTaskInstance.State == TaskInstanceConst.State.Released)
            //{
            //    return false;
            //}
            //if (car.CurrTaskInstance.State == TaskInstanceConst.State.Running)
            //{
            //    return false;
            //}
            if (car.CurrTaskInstanceProcess == null)
            {
                return false;
            }
            if (car.CurrNode == car.EndNode)
            {

            }
            else
            {
                if (car.CurrTaskInstance.State != TaskInstanceConst.State.Canceling && car.CurrTaskInstance.State != TaskInstanceConst.State.Released && car.CurrTaskInstance.State != TaskInstanceConst.State.Running)
                {
                    _carSessionService.SendTask(car.ToCarTask());
                }
            }
            if (car.CurrTaskInstanceAction is null)
            {
                return false;
            }
            if (car.CurrNode != (car.CurrTaskInstanceAction.TaskInstanceProcess.Node ?? car.CurrTaskInstanceAction.TaskInstanceProcess.Edge?.StartNode))
            {
                return false;
            }
            if (!car.CurrTaskInstanceAction.TaskInstanceParameters.Any())
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数");
                return false;
            }
            if (car.CarState is null || !car.CarState.TryJsonTo<CarState>(out var carState))
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 车辆状态错误");
                return false;
            }
            var defaultValue = car.CurrTaskInstanceAction.TaskInstanceParameters.FirstOrDefault(e => e.Key.Equals("Default", StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数 [Default]");
                return false;
            }
            return true;
        }

        public void ScanTrafficRecord(Car car)
        {
            if (car.CarState is null || !car.CarState.TryJsonTo<CarState>(out var carState) || carState == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 车辆状态错误");
                return;
            }

            if (_firstLoad)
            {
                //首次加载，清除交管信息
                _firstLoad = false;
                _trafficService.DeleteByCarCode(car.Code);
            }

            string blockedByStr = string.Join(";", carState.BlockedBy.Select(e => { return $"({e.CarCode},{e.Type})"; }));

            if (!string.IsNullOrEmpty(blockedByStr) && string.IsNullOrEmpty(_carTrafficInfo))
            {
                //写入交管信息
                var mode = new Traffic
                {
                    FromCarCode = car.Code,
                    FromCarName = car.Name,
                    ToCarCode = carState.BlockedBy[0].CarCode,
                    ToCarName = _carService.BootService.Cars.Where(e => e.Code == carState.BlockedBy[0].CarCode).FirstOrDefault()?.Name,
                    Info = blockedByStr,
                    LockedNodes = carState.AquiringLock.ToString(),
                    State = carState.TrafficMessage,
                    StartTime = DateTime.Parse(carState.BlockingTime!)
                };
                _trafficService.AddModel(mode);
            }
            else if (string.IsNullOrEmpty(blockedByStr) && !string.IsNullOrEmpty(_carTrafficInfo))
            {
                //清空交管信息
                _trafficService.DeleteByCarCode(car.Code);
            }
            else if (!carState.BlockedBy.Equals(_carTrafficInfo) && !string.IsNullOrEmpty(blockedByStr) && !string.IsNullOrEmpty(_carTrafficInfo))
            {
                //更新交管信息
                var mode = new Traffic
                {
                    FromCarCode = car.Code,
                    ToCarCode = carState.BlockedBy[0].CarCode,
                    ToCarName = _carService.BootService.Cars.Where(e => e.Code == carState.BlockedBy[0].CarCode).FirstOrDefault()?.Name,
                    LockedNodes = carState.AquiringLock.ToString(),
                    State = carState.TrafficMessage,
                    Info = blockedByStr,
                    StartTime = DateTime.Parse(carState.BlockingTime!)
                };
                _trafficService.UpdateModel(mode);
            }
            else { }
            _carTrafficInfo = blockedByStr;
        }

    }
}