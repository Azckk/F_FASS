using DotNetCore.CAP;
using FASS.Web.Api.Hubs.Monitor;

namespace FASS.Web.Api.Services.EventBus.Subscribes
{
    public class DefaultSubscribe : ICapSubscribe
    {
        public EventBusService EventBusService { get; }

        public DefaultSubscribe(
            EventBusService eventBusService)
        {
            EventBusService = eventBusService;
        }

        //[CapSubscribe("CarController.Enable")]
        //public void CarEnable(List<string> keyValues)
        //{

        //}

        //[CapSubscribe("CarController.Disable")]
        //public void CarDisable(List<string> keyValues)
        //{

        //}

        [CapSubscribe("MdcsController.GlobalAlarm")]
        public void GlobalAlarm(string info)
        {
            EventBusService.Logger.LogWarning($"GlobalAlarm.Info :{info}");
            AlarmHub.Message = info;
            AlarmHub.CreateAt = DateTime.Now;
        }
    }
}