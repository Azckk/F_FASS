using FASS.Service.Services.Screen.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FASS.Web.Api.Hubs.Screen
{
    public class DataHub : Hub
    {
        private readonly ILogger<DataHub> _logger;
        private readonly IDataService _dataService;

        public DataHub(
            ILogger<DataHub> logger,
            IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        public async Task SendMessage(string user, string message)
        {
            var data = new
            {
                data1 = _dataService.getData1(),
                data2 = _dataService.getData2(),
                data3 = _dataService.getData3(),
                data4 = _dataService.getData4(),
                data5 = _dataService.getData5(),
                data6 = _dataService.getData6()
            };
            await Clients.Caller.SendAsync("ReceiveMessage", user, data);
        }
    }
}