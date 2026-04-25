using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Boot.Services.Cars;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Data.Consts.Base;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Instant;
using FASS.Data.Models.Data;
using FASS.Extend.Car.Fairyland.Pcb;
using FASS.Scheduler.Models;
using FASS.Service.Consts.Data;
using CarActionConst = FASS.Service.Consts.Data.CarActionConst;

namespace FASS.Scheduler.Services.Cars.Fairyland.Pcb
{
    public class CarRequestService : ICarRequestService
    {
        private readonly ILogger<CarRequestService> _logger;
        private readonly AppSettings _appSettings;
        private CarService _carService = null!;
        private CarSessionService _carSessionService = null!;
        private ulong _task = DateTime.Now.Ticks.ToULong();
        private ulong _actionId = DateTime.Now.Ticks.ToULong();

        public CarRequestService(
            ILogger<CarRequestService> logger,
            AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public void Initialize(CarService carService)
        {
            _carService = carService;
            _carSessionService = (CarSessionService)_carService.CarSessionService;
        }

        public bool TryExecute(Car car, out Car result)
        {
            result = car;
            try
            {
                TryGetCarInstantAction(car, out result);
                TryGetTaskInstance(car, out result);
                Thread.Sleep(_appSettings.Scheduler.StateDueTime);
                _carSessionService.AutoResetEvent.WaitOne();
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
                _carSessionService.SendControlStart(car.Code.ToUShort());
                car.NextState = CarConst.State.Running;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Stop && car.CurrState != CarConst.State.Stopping)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendControlStop(car.Code.ToUShort());
                car.NextState = CarConst.State.Stopping;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.EmergencyStop && car.CurrState != CarConst.State.EmergencyStop)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendControlEmergencyStop(car.Code.ToUShort());
                car.NextState = CarConst.State.EmergencyStop;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Reset && (car.CurrState == CarConst.State.EmergencyStop || car.CurrState == CarConst.State.Faulting))
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendControlReset(car.Code.ToUShort());
                car.NextState = CarConst.State.Stopping;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Dormancy && car.CurrState != CarConstExtend.State.Dormancying)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendControlRest(car.Code.ToUShort());
                car.NextState = CarConstExtend.State.Dormancying;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.ShutOff && car.CurrState != CarConstExtend.State.Shutdown)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendControlShutdown(car.Code.ToUShort());
                car.NextState = CarConstExtend.State.Shutdown;
                return false;
            }
            if (car.CurrNode is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] [当前站点] 未设置参数");
                return false;
            }
            if (!car.CurrCarInstantAction.CarInstantParameters.Any())
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 未设置参数");
                return false;
            }
            if (car.CarState is null || !car.CarState.TryJsonTo<ReceiveStateMessage>(out var recvStateMessage) || recvStateMessage?.NodeMessage is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 车辆状态错误");
                return false;
            }
            var carState = recvStateMessage.NodeMessage;
            var defaultValue = car.CurrCarInstantAction.CarInstantParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                return false;
            }
            //添加动作时间戳
            if (string.IsNullOrEmpty(car.CurrCarInstantAction.Extend))
            {
                _actionId = DateTime.Now.Ticks.ToULong();
                car.CurrCarInstantAction.Extend = _actionId.ToString();
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.StartStop && carState.StartStop.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionStartStop(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Direction && carState.Direction.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionDirection(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Orientation && carState.Orientation.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionOrientation(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Byroad && carState.Byroad.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionByroad(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Speed && carState.Speed.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionSpeed(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), (ushort)(defaultValue.ToUShort() * 10), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Obstacle && carState.Obstacle.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionObstacle(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Audio && carState.Audio.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionAudio(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Light && carState.Light.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionLight(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Charge)
            {
                // 下发充电时长或停止充电指令时，动作结束判定标识均为停止充电(carState.Charge = 1)
                if ((defaultValue.ToByte() > 2 || defaultValue.ToByte() == 1) && carState.Charge.ToString() != "1")
                {
                    car.CurrCarInstantAction.State = CarConst.State.Running;
                    _carSessionService.SendActionCharge(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                    return false;
                }
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.StartCharging && carState.Charge.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionCharge(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.StopCharging && carState.Charge.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionCharge(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Rest && carState.Rest.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionRest(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Lift && carState.Lift.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionLift(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Clamp && carState.Clamp.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionClamp(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Tray && carState.Tray.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionTray(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Roll && carState.Roll.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionRoll(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Shutdown && carState.Shutdown.ToString() != defaultValue)
            {
                car.CurrCarInstantAction.State = CarConst.State.Running;
                _carSessionService.SendActionShutdown(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrCarInstantAction.Extend.ToULong());
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
                _carSessionService.SendControlStop(car.Code.ToUShort());
                return false;
            }
            if (car.CurrTaskInstance.State == TaskInstanceConst.State.Resumed)
            {
                _carSessionService.SendControlStart(car.Code.ToUShort());
                return false;
            }
            if (car.CurrTaskInstanceProcess is null)
            {
                return false;
            }
            if (car.CurrNode == car.EndNode)
            {
                _task = DateTime.Now.Ticks.ToULong();
            }
            else
            {
                var sendNodes = GetSendNodes(car);
                _carSessionService.SendNodes(car.Code.ToUShort(), _task, sendNodes.Count.ToUShort(), sendNodes.ToArray());
            }
            if (car.CurrTaskInstanceAction is null)
            {
                return false;
            }
            if (car.CurrNode != (car.CurrTaskInstanceAction.TaskInstanceProcess.Node ?? car.CurrTaskInstanceAction.TaskInstanceProcess.Edge?.StartNode))
            {
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Start && car.CurrState != CarConst.State.Running)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendControlStart(car.Code.ToUShort());
                car.NextState = CarConst.State.Running;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Stop && car.CurrState != CarConst.State.Stopping)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendControlStop(car.Code.ToUShort());
                car.NextState = CarConst.State.Stopping;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.EmergencyStop && car.CurrState != CarConst.State.EmergencyStop)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendControlEmergencyStop(car.Code.ToUShort());
                car.NextState = CarConst.State.EmergencyStop;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Reset && (car.CurrState == CarConst.State.EmergencyStop || car.CurrState == CarConst.State.Faulting))
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendControlReset(car.Code.ToUShort());
                car.NextState = CarConst.State.Stopping;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Dormancy && car.CurrState != CarConstExtend.State.Dormancying)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendControlRest(car.Code.ToUShort());
                car.NextState = CarConstExtend.State.Dormancying;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Shutdown && car.CurrState != CarConstExtend.State.Shutdown)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendControlShutdown(car.Code.ToUShort());
                car.NextState = CarConstExtend.State.Shutdown;
                return false;
            }
            if (car.CurrNode is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] [当前站点] 未设置参数");
                return false;
            }
            if (!car.CurrTaskInstanceAction.TaskInstanceParameters.Any())
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数");
                return false;
            }
            if (car.CarState is null || !car.CarState.TryJsonTo<ReceiveStateMessage>(out var recvStateMessage) || recvStateMessage?.NodeMessage == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 车辆状态错误");
                return false;
            }
            var carState = recvStateMessage.NodeMessage;
            var defaultValue = car.CurrTaskInstanceAction.TaskInstanceParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                return false;
            }
            //添加动作时间戳
            if (string.IsNullOrEmpty(car.CurrTaskInstanceAction.Extend))
            {
                _actionId = DateTime.Now.Ticks.ToULong();
                car.CurrTaskInstanceAction.Extend = _actionId.ToString();
            }

            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.StartStop && carState.StartStop.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionStartStop(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Direction && carState.Direction.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionDirection(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Orientation && carState.Orientation.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionOrientation(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Byroad && carState.Byroad.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionByroad(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Speed && carState.Speed.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionSpeed(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), (ushort)(defaultValue.ToUShort() * 10), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Obstacle && carState.Obstacle.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionObstacle(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Audio && carState.Audio.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionAudio(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Light && carState.Light.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionLight(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Charge)
            {
                // 下发充电时长或停止充电指令时，动作结束判定标识均为停止充电(carState.Charge = 1)
                if ((defaultValue.ToByte() > 2 || defaultValue.ToByte() == 1) && carState.Charge.ToString() != "1")
                {
                    car.CurrTaskInstanceAction.State = CarConst.State.Running;
                    _carSessionService.SendActionCharge(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                    return false;
                }
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.StartCharging && carState.Charge.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionCharge(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.StopCharging && carState.Charge.ToString() != defaultValue)
            {
                var CarMaxBattery = _carService.BootService.ConfigData.CarMaxBattery?.ToInt();
                if (car.Battery >= car.MaxBattery && car.Battery >= CarMaxBattery)
                {
                    car.CurrTaskInstanceAction.State = CarConst.State.Running;
                    _carSessionService.SendActionCharge(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                    return false;
                }
            }

            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Rest && carState.Rest.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionRest(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Lift && carState.Lift.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionLift(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Clamp && carState.Clamp.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionClamp(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Tray && carState.Tray.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionTray(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Roll && carState.Roll.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionRoll(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Shutdown && carState.Shutdown.ToString() != defaultValue)
            {
                car.CurrTaskInstanceAction.State = CarConst.State.Running;
                _carSessionService.SendActionShutdown(car.Code.ToUShort(), car.CurrNode.Code.ToUShort(), defaultValue.ToByte(), car.CurrTaskInstanceAction.Extend.ToULong());
                return false;
            }
            return true;
        }

        public List<NodeMessage> GetSendNodes(Car car)
        {
            var carNodes = car.CarNodes.Where(e => e.Type == CarNodeConst.Type.Curr).ToList();
            var carEdges = car.CarEdges.Where(e => e.Type == CarEdgeConst.Type.Curr).ToList();
            var nodeMessages = new List<NodeMessage>(carNodes.Count);
            foreach (var carNode in carNodes)
            {
                var node = carNode.Node;
                var nodeMessage = new NodeMessage();
                nodeMessage.Node = node.Code.ToUShort();
                //nodeMessage.StartStop = (node == car.EndNode ? 2 : 1).ToByte();
                nodeMessage.StartStop = (node == carNodes.Last().Node ? 2 : 1).ToByte();
                if (node.NodeActions is not null)
                {
                    foreach (var nodeAction in node.NodeActions)
                    {
                        if (!nodeAction.NodeActionParameters.Any())
                        {
                            _logger.LogWarning($"车辆 [{car.Code}] 站点 [{node.Code}] 站点动作类型 [{nodeAction.ActionType}] 未设置参数");
                            continue;
                        }
                        var defaultValue = nodeAction.NodeActionParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
                        if (defaultValue is null)
                        {
                            _logger.LogWarning($"车辆 [{car.Code}] 站点 [{node.Code}] 站点动作类型 [{nodeAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Byroad)
                        {
                            nodeMessage.Byroad = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Obstacle)
                        {
                            nodeMessage.Obstacle = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Audio)
                        {
                            nodeMessage.Audio = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Light)
                        {
                            nodeMessage.Light = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Charge)
                        {
                            nodeMessage.Charge = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Rest)
                        {
                            nodeMessage.Rest = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Lift)
                        {
                            nodeMessage.Lift = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Clamp)
                        {
                            nodeMessage.Clamp = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Tray)
                        {
                            nodeMessage.Tray = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Roll)
                        {
                            nodeMessage.Roll = defaultValue.ToByte();
                            continue;
                        }
                        if (nodeAction.ActionType == CarActionConst.Type.Shutdown)
                        {
                            nodeMessage.Shutdown = defaultValue.ToByte();
                            continue;
                        }
                    }
                }
                var edge = carEdges.FirstOrDefault(e => e.Edge.StartNode == node)?.Edge;
                if (edge is not null)
                {
                    nodeMessage.Speed = (edge.MaxSpeed * 10).ToUShort();
                    if (edge.StartNode == car.StartNode)
                    {
                        var startSpeedValue = edge.StartNode.NodeExtends.FirstOrDefault(e => e.Key == EdgeConst.Extend.StartSpeed)?.Value;
                        if (startSpeedValue is not null && ushort.TryParse(startSpeedValue, out var startSpeed))
                        {
                            nodeMessage.Speed = (ushort)(startSpeed * 10);
                        }
                    }
                    if (edge.EndNode == car.EndNode)
                    {
                        var stopSpeedValue = edge.EndNode.NodeExtends.FirstOrDefault(e => e.Key == EdgeConst.Extend.StopSpeed)?.Value;
                        if (stopSpeedValue is not null && ushort.TryParse(stopSpeedValue, out var stopSpeed))
                        {
                            nodeMessage.Speed = (ushort)(stopSpeed * 10);
                        }
                    }
                    nodeMessage.Distance = edge.Length.ToUShort();
                    var edgeAngle = MathHelper.PointToAngle(edge.StartNode.NodePosition.X, edge.StartNode.NodePosition.Y, edge.EndNode.NodePosition.X, edge.EndNode.NodePosition.Y);
                    var carAngle = MathHelper.NormalizeAngle(car.Theta);
                    //nodeMessage.Direction = MathHelper.AngleToRotate(carAngle, edgeAngle).ToByte(); //无功能
                    nodeMessage.Orientation = edge.Orientation.ToByte();
                    var carOrientationExtend = edge.EdgeExtends.FirstOrDefault(e => e.Key == EdgeConst.Extend.CarOrientation);
                    if (carOrientationExtend is not null)
                    {
                        var carOrientations = carOrientationExtend.Value?.JsonTo<List<Models.EdgeCarOrientation>>();
                        var carOrientation = carOrientations?.FirstOrDefault(e => e.CarCode == car.Code.ToString());
                        if (carOrientation is not null && byte.TryParse(carOrientation.Orientation, out var orientation))
                        {
                            nodeMessage.Orientation = orientation;
                        }
                    }
                    if (nodeMessage.Byroad == 0)
                    {
                        //站点上没有配置岔道动作，执行地图路线配置的岔道动作
                        nodeMessage.Byroad = (string.IsNullOrEmpty(edge.Direction) ? "0" : edge.Direction).ToByte();
                    }
                    var carDirectionExtend = edge.EdgeExtends.FirstOrDefault(e => e.Key == EdgeConst.Extend.CarDirection);
                    if (carDirectionExtend is not null)
                    {
                        var carDirections = carDirectionExtend.Value?.JsonTo<List<Models.EdgeCarDirection>>();
                        var carDirection = carDirections?.FirstOrDefault(e => e.CarCode == car.Code.ToString());
                        if (carDirection is not null && byte.TryParse(carDirection.Direction, out var byroad))
                        {
                            nodeMessage.Byroad = byroad;
                        }
                    }
                    if (edge.EdgeActions is not null)
                    {
                        foreach (var edgeAction in edge.EdgeActions)
                        {
                            if (!edgeAction.EdgeActionParameters.Any())
                            {
                                _logger.LogWarning($"车辆 [{car.Code}] 路线 [{edge.Code}] 路线动作类型 [{edgeAction.ActionType}] 未设置参数");
                                continue;
                            }
                            var defaultValue = edgeAction.EdgeActionParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
                            if (defaultValue is null)
                            {
                                _logger.LogWarning($"车辆 [{car.Code}] 路线 [{edge.Code}] 路线动作类型 [{edgeAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Byroad)
                            {
                                nodeMessage.Byroad = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Obstacle)
                            {
                                nodeMessage.Obstacle = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Audio)
                            {
                                nodeMessage.Audio = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Light)
                            {
                                nodeMessage.Light = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Charge)
                            {
                                nodeMessage.Charge = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Rest)
                            {
                                nodeMessage.Rest = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Lift)
                            {
                                nodeMessage.Lift = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Clamp)
                            {
                                nodeMessage.Clamp = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Tray)
                            {
                                nodeMessage.Tray = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Roll)
                            {
                                nodeMessage.Roll = defaultValue.ToByte();
                                continue;
                            }
                            if (edgeAction.ActionType == CarActionConst.Type.Shutdown)
                            {
                                nodeMessage.Shutdown = defaultValue.ToByte();
                                continue;
                            }
                        }
                    }
                }
                nodeMessages.Add(nodeMessage);
            }
            return nodeMessages;
        }
    }
}