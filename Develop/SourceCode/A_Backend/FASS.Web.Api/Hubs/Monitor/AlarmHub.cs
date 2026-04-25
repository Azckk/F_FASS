using Microsoft.AspNetCore.SignalR;

namespace FASS.Web.Api.Hubs.Monitor
{
    public class AlarmHub : Hub
    {
        private readonly ILogger<AlarmHub> _logger;
        public static string Message = string.Empty;
        public static DateTime CreateAt = DateTime.Now;

        public AlarmHub(
            ILogger<AlarmHub> logger
            )
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task Update()
        {
            Thread.Sleep(500);
            try
            {
                if (DateTime.Compare(CreateAt.AddSeconds(2), DateTime.Now) > 0)
                {
                    await Clients.Caller.SendAsync("Alarm", Message);
                }
                else
                {
                    await Clients.Caller.SendAsync("Alarm", "");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
