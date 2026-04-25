using System.Net.Http.Headers;
using HttpClient = Common.Net.Http.HttpClient;

namespace FASS.Extend.Car.Fairyland.Pc
{
    public class Command
    {
        public HttpClient Client { get; private set; }
        public Command(Uri uri)
        {
            Client = new HttpClient()
            {
                BaseAddress = uri,
                AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", "Token"),
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                Timeout = TimeSpan.FromSeconds(5)
            };
            Client.Initialize();
        }

        public string Request(string url, string requestString)
        {
            var response = Client.PostAsJsonAsync(url, requestString).GetAwaiter().GetResult();
            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseString;
        }

        public string Start(string request) => Request("/car/start", request);
        public string Stop(string request) => Request("/car/stop", request);
        public string EmergencyStop(string request) => Request("/car/emergencyStop", request);
        public string Reset(string request) => Request("/car/reset", request);
        public string Action(string request) => Request("/car/action", request);
        public string Task(string request) => Request("/car/carTask", request);
        public string State(string request) => Request("/car/carState", request);
    }
}