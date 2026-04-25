using Common.NETCore;
using HttpClient = Common.Net.Http.HttpClient;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendHttpClientService
    {
        public ExtendService ExtendService { get; }

        public bool IsStarted { get; private set; }

        public HttpClient HttpClient { get; private set; } = null!;

        public ExtendHttpClientService(
            ExtendService extendService)
        {
            ExtendService = extendService;

            Init();
        }

        public void Init()
        {
            var httpClientBaseAddress = Guard.NotNull(ExtendService.AppSettings.Extend.HttpClientBaseAddress);
            HttpClient = new HttpClient() { BaseAddress = new Uri(httpClientBaseAddress) };
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
                Task.Run(() => Keepalive());
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                if (!IsStarted)
                {
                    return;
                }
                IsStarted = false;
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public async Task HttpGetAsync(object? param = null)
        {
            var response = await HttpClient.GetAsTextAsync("/test/get", param);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            ExtendService.Logger.LogError($"Get:[{result}]");

        }

        public async Task HttpPostAsync(object? param = null)
        {
            var response = await HttpClient.PostAsTextAsync("/test/post", param);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            ExtendService.Logger.LogError($"Post:[{result}]");
        }

        private async Task Keepalive()
        {
            while (true)
            {
                if (!IsStarted)
                {
                    break;
                }
                try
                {
                    var message = "ACK";
                    await HttpPostAsync(new { data = message });
                }
                catch (Exception ex)
                {
                    ExtendService.Logger.LogError(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}