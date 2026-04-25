using Common.NETCore.Extensions;
using FASS.Boot.Services.Cars;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Data.Consts.Data;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Instant;
using FASS.Data.Models.Data;
using FASS.Extend.Car.Fairyland.Pc.Models.Response;
using CarActionConst = FASS.Service.Consts.Data.CarActionConst;

namespace FASS.Scheduler.Services.Cars.Fairyland.Pc
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
            if (car.CurrCarInstantAction.ActionType == CarActionConst.Type.Reset && car.CurrState == CarConst.State.Running)
            {
                car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
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
            var defaultValue = car.CurrCarInstantAction.CarInstantParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue == null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 即时动作 动作类型 [{car.CurrCarInstantAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                return false;
            }
            //var carAction = carState.Actions.FirstOrDefault(e => e.Code == car.CurrCarInstantAction.Id);
            //if (carAction != null && carAction.State == CarInstantActionConst.State.Completed)
            //{
            //    car.CurrCarInstantAction.State = CarInstantActionConst.State.Completing;
            //    return false;
            //}
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
            if (car.CurrTaskInstanceAction.ActionType == CarActionConst.Type.Reset && car.CurrState == CarConst.State.Running)
            {
                car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
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
            var defaultValue = car.CurrTaskInstanceAction.TaskInstanceParameters.FirstOrDefault(e => e.Key.Equals(Data.Consts.Data.CarActionConst.ParameterKey.Default, StringComparison.OrdinalIgnoreCase))?.Value;
            if (defaultValue is null)
            {
                _logger.LogWarning($"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}] 任务实例 [{car.CurrTaskInstance.Code}] 子任务 [{car.CurrTaskInstanceProcess.Code}] 动作类型 [{car.CurrTaskInstanceAction.ActionType}] 未设置参数 [{Data.Consts.Data.CarActionConst.ParameterKey.Default}]");
                return false;
            }
            //var carAction = carState.Tasks.FirstOrDefault(e => e.Code == car.CurrTaskInstance.Id)?.Nodes.FirstOrDefault(e => e.Code == car.CurrTaskInstanceProcess.Node.Code)?.Actions.FirstOrDefault(e => e.Code == car.CurrTaskInstanceAction.Id);
            //if (carAction != null && carAction.State == CarInstantActionConst.State.Completed)
            //{
            //    car.CurrTaskInstanceAction.State = TaskInstanceActionConst.State.Completing;
            //    return false;
            //}
            return true;
        }
    }
}