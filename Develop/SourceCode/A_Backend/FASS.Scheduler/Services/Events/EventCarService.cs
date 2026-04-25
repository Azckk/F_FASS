using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Boot.Models.Events;
using FASS.Boot.Services;
using FASS.Data.Consts.Data;
using FASS.Data.Models.Data;

namespace FASS.Scheduler.Services.Events
{
    public class EventCarService
    {
        public EventService EventService { get; }

        public IBootService BootService { get; }

        public bool IsStarted { get; private set; }

        public List<EventCar> EventCars { get; private set; } = [];

        public EventCarService(EventService eventService)
        {
            EventService = eventService;

            BootService = EventService.ServiceProvider.GetRequiredService<IBootService>();

            Init();
        }

        public void Init()
        {
            var carConfig = Guard.NotNull(EventService.AppSettings.Event.CarConfig);
            if (File.Exists(carConfig))
            {
                EventCars = File.ReadAllText(carConfig).JsonTo<List<EventCar>>()?.Where(e => e.TriggerEnable).ToList() ?? [];
            }
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
            BootService.CarTimerTick += ExecuteHandler;
        }

        private void SetStop()
        {
            BootService.CarTimerTick = null!;
        }

        private void ExecuteHandler(Car prevCar, Car car)
        {
            if (EventCars == null || !EventCars.Any())
            {
                return;
            }
            Task.Run(() =>
            {
                var timerTickEvents = EventCars.Where(e => e.TriggerEvent == TriggerEvent.TimerTick).ToList();
                var triggerEvents = timerTickEvents.Where(e => TriggerHandler(car, e)).ToList();
                var conditionEvents = triggerEvents.Where(e => ConditionHandler(car, e)).ToList();
                var actionEvents = conditionEvents.Where(e => ActionHandler(car, e)).ToList();
                actionEvents.ForEach(e =>
                {
                    if (e.ActionType is not null)
                    {
                        BootService.AddCarInstantAction(car.Id, e.ActionType, $"车辆 [{car.Code}] 站点 [{car.CurrNode?.Code}]");
                    }
                });
            });
        }

        private bool TriggerHandler(Car car, EventCar e)
        {
            if (e.TriggerCarCode?.Split(',').Contains(car.Code) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarPrevNodeCode?.Split(',').Contains(car.PrevNode?.Code) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarCurrNodeCode?.Split(',').Contains(car.CurrNode?.Code) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarNextNodeCode?.Split(',').Contains(car.NextNode?.Code) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarPrevState?.Split(',').Contains(car.PrevState) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarCurrState?.Split(',').Contains(car.CurrState) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarNextState?.Split(',').Contains(car.NextState) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarCharge?.Split(',').Any(e => e.ToDouble() == car.Battery) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarSpeed?.Split(',').Any(e => e.ToDouble() == car.Speed) is not null and true)
            {
                return true;
            }
            if (e.TriggerCarTheta?.Split(',').Any(e => e.ToDouble() == car.Theta) is not null and true)
            {
                return true;
            }
            return false;
        }

        private bool ConditionHandler(Car car, EventCar e)
        {
            if (!e.ConditionEnable)
            {
                return true;
            }
            if (e.ConditionType == ConditionType.Method)
            {
                return MethodInvok(car, e);
            }
            if (e.ConditionType == ConditionType.Assembly && e.ConditionAssemblyFile is not null && e.ConditionTypeName is not null && e.ConditionTypeArgs is not null && e.ConditionMethodName is not null && e.ConditionMethodArgs is not null)
            {
                return (bool)(ReflectHelper.AssemblyInvoke(e.ConditionAssemblyFile, e.ConditionTypeName, e.ConditionTypeArgs.Split(','), e.ConditionMethodName, e.ConditionMethodArgs.Split(',')) ?? false);
            }
            if (e.ConditionType == ConditionType.Plugin && e.ConditionTypeName is not null && e.ConditionTypeArgs is not null && e.ConditionMethodName is not null && e.ConditionMethodArgs is not null)
            {
                return (bool)(ReflectHelper.PluginInvoke(e.ConditionTypeName, e.ConditionTypeArgs.Split(','), e.ConditionMethodName, e.ConditionMethodArgs.Split(',')) ?? false);
            }
            return false;
        }

        private bool ActionHandler(Car car, EventCar e)
        {
            if (e.ActionEnable)
            {
                return true;
            }
            return false;
        }

        private bool MethodInvok(Car car, EventCar e)
        {
            return e.ConditionMethodName switch
            {
                "AnyOccupyNodeWithCar" => AnyOccupyNodeWithCar(car, e),
                "AnyOccupyNodeWithCarCode" => AnyOccupyNodeWithCarCode(car, e),
                "AnyOccupyNodeWithCarState" => AnyOccupyNodeWithCarState(car, e),
                "AnyOccupyNodeWithCarCodeOrCarState" => AnyOccupyNodeWithCarCodeOrCarState(car, e),
                "AnyOccupyNodeWithoutCarCode" => AnyOccupyNodeWithoutCarCode(car, e),
                "AnyOccupyNodeWithoutCarState" => AnyOccupyNodeWithoutCarState(car, e),
                "AnyOccupyNodeWithoutCarCodeAndCarState" => AnyOccupyNodeWithoutCarCodeAndCarState(car, e),
                "AllNotOccupyNodeWithCar" => AllNotOccupyNodeWithCar(car, e),
                "AllNotOccupyNodeWithCarCode" => AllNotOccupyNodeWithCarCode(car, e),
                "AllNotOccupyNodeWithCarState" => AllNotOccupyNodeWithCarState(car, e),
                "AllNotOccupyNodeWithCarCodeOrCarState" => AllNotOccupyNodeWithCarCodeOrCarState(car, e),
                "AllNotOccupyNodeWithoutCarCode" => AllNotOccupyNodeWithoutCarCode(car, e),
                "AllNotOccupyNodeWithoutCarState" => AllNotOccupyNodeWithoutCarState(car, e),
                "AllNotOccupyNodeWithoutCarCodeAndCarState" => AllNotOccupyNodeWithoutCarCodeAndCarState(car, e),
                _ => false
            };
        }

        private bool AnyOccupyNodeWithCar(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithCar(e.ConditionMethodArgs.Split(','));
            }
            return false;
        }

        private bool AnyOccupyNodeWithCarCode(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithCarCode(e.ConditionMethodArgs.Split(','), car.Code);
            }
            return false;
        }

        private bool AnyOccupyNodeWithCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithCarState(e.ConditionMethodArgs.Split(','), CarConst.State.Running);
            }
            return false;
        }

        private bool AnyOccupyNodeWithCarCodeOrCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithCarCodeOrCarState(e.ConditionMethodArgs.Split(','), car.Code, CarConst.State.Running);
            }
            return false;
        }

        private bool AnyOccupyNodeWithoutCarCode(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithoutCarCode(e.ConditionMethodArgs.Split(','), car.Code);
            }
            return false;
        }

        private bool AnyOccupyNodeWithoutCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithoutCarState(e.ConditionMethodArgs.Split(','), CarConst.State.Running);
            }
            return false;
        }

        private bool AnyOccupyNodeWithoutCarCodeAndCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AnyOccupyNodeWithoutCarCodeAndCarState(e.ConditionMethodArgs.Split(','), car.Code, CarConst.State.Running);
            }
            return false;
        }

        private bool AllNotOccupyNodeWithCar(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithCar(e.ConditionMethodArgs.Split(','));
            }
            return false;
        }

        private bool AllNotOccupyNodeWithCarCode(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithCarCode(e.ConditionMethodArgs.Split(','), car.Code);
            }
            return false;
        }

        private bool AllNotOccupyNodeWithCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithCarState(e.ConditionMethodArgs.Split(','), CarConst.State.Running);
            }
            return false;
        }

        private bool AllNotOccupyNodeWithCarCodeOrCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithCarCodeOrCarState(e.ConditionMethodArgs.Split(','), car.Code, CarConst.State.Running);
            }
            return false;
        }

        private bool AllNotOccupyNodeWithoutCarCode(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithoutCarCode(e.ConditionMethodArgs.Split(','), car.Code);
            }
            return false;
        }

        private bool AllNotOccupyNodeWithoutCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithoutCarState(e.ConditionMethodArgs.Split(','), CarConst.State.Running);
            }
            return false;
        }

        private bool AllNotOccupyNodeWithoutCarCodeAndCarState(Car car, EventCar e)
        {
            if (e.ConditionMethodArgs is not null)
            {
                return BootService.AllNotOccupyNodeWithoutCarCodeAndCarState(e.ConditionMethodArgs.Split(','), car.Code, CarConst.State.Running);
            }
            return false;
        }
    }
}
