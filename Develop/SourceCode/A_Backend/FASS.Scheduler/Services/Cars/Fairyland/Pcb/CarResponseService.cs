using Common.NETCore.Extensions;
using FASS.Boot.Services.Cars;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Instant;
using FASS.Data.Models.Data;
using FASS.Extend.Car.Fairyland.Pcb;
using FASS.Service.Consts.Data;
using CarActionConst = FASS.Service.Consts.Data.CarActionConst;

namespace FASS.Scheduler.Services.Cars.Fairyland.Pcb
{
    public class CarResponseService : ICarResponseService
    {
        private readonly ILogger<CarResponseService> _logger;
        private CarService _carService = null!;
        private CarSessionService _carSessionService = null!;

        public CarResponseService(
            ILogger<CarResponseService> logger)
        {
            _logger = logger;
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
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Start && car.CurrState == CarConst.State.Running)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Stop && car.CurrState == CarConst.State.Stopping)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.EmergencyStop && car.CurrState == CarConst.State.EmergencyStop)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Reset && car.CurrState != CarConst.State.EmergencyStop && car.CurrState != CarConst.State.Faulting)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Dormancy && car.CurrState == CarConstExtend.State.Dormancying)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.ShutOff && car.CurrState == CarConstExtend.State.Shutdown)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
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
            if (car.CarState is null || !car.CarState.TryJsonTo<ReceiveStateMessage>(out var recvStateMessage) || recvStateMessage?.NodeMessage == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 车辆状态错误");
                return false;
            }
            var carState = recvStateMessage.NodeMessage;
            var defaultValue = car.CurrCarInstantAction.CarInstantParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.StartStop && carState.StartStop.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Direction && carState.Direction.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Orientation && carState.Orientation.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Byroad && carState.Byroad.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Speed && carState.Speed * 0.1 == double.Parse(defaultValue))
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Obstacle && carState.Obstacle.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Audio && carState.Audio.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Light && carState.Light.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Charge)
            {
                if ((defaultValue.ToByte() > 2 || defaultValue.ToByte() == 1) && carState.Charge.ToString() == "1")
                {
                    car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                    return false;
                }
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.StartCharging && carState.Charge.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.StopCharging && carState.Charge.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Rest && carState.Rest.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Lift && carState.Lift.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Clamp && carState.Clamp.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Tray && carState.Tray.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Roll && carState.Roll.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
                return false;
            }
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Shutdown && carState.Shutdown.ToString() == defaultValue)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
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
            if (car.CurrTaskInstanceProcess is null)
            {
                return false;
            }
            if (car.CurrTaskInstanceAction is null)
            {
                return false;
            }
            if (car.CurrNode != (car.CurrTaskInstanceAction.TaskInstanceProcess.Node ?? car.CurrTaskInstanceAction.TaskInstanceProcess.Edge?.StartNode))
            {
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Start && car.CurrState == CarConst.State.Running)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Stop && car.CurrState == CarConst.State.Stopping)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.EmergencyStop && car.CurrState == CarConst.State.EmergencyStop)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Reset && car.CurrState != CarConst.State.EmergencyStop && car.CurrState != CarConst.State.Faulting)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Dormancy && car.CurrState == CarConstExtend.State.Dormancying)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.ShutOff && car.CurrState == CarConstExtend.State.Shutdown)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (!car.CurrTaskInstanceAction.TaskInstanceParameters.Any())
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数");
                return false;
            }
            if (car.CarState is null || !car.CarState.TryJsonTo<ReceiveStateMessage>(out var recvStateMessage) || recvStateMessage?.NodeMessage == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 车辆状态错误");
                return false;
            }
            var carState = recvStateMessage.NodeMessage;
            var defaultValue = car.CurrTaskInstanceAction.TaskInstanceParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.StartStop && carState.StartStop.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Direction && carState.Direction.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Orientation && carState.Orientation.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Byroad && carState.Byroad.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Speed && carState.Speed * 0.1 == double.Parse(defaultValue))
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Obstacle && carState.Obstacle.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Audio && carState.Audio.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Light && carState.Light.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Charge)
            {
                //停止充电(Charge == 1),结束充电动作
                if ((defaultValue.ToByte() > 2 || defaultValue.ToByte() == 1) && carState.Charge.ToString() == "1")
                {
                    car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                    return false;
                }
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.StartCharging && carState.Charge.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.StopCharging && carState.Charge.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Rest && carState.Rest.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Lift && carState.Lift.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Clamp && carState.Clamp.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Tray && carState.Tray.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Roll && carState.Roll.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Shutdown && carState.Shutdown.ToString() == defaultValue)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
                return false;
            }
            return true;
        }
    }
}