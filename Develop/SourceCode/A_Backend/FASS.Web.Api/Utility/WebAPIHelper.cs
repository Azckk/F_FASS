using Serilog;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FASS.Web.Api.Utility
{
    public class WebAPIHelper
    {
        private static WebAPIHelper instance = null;

        public static WebAPIHelper Instance
        {
            get
            {
                return instance ?? (instance = new WebAPIHelper());
            }
        }

        private ConcurrentDictionary<string, HttpClient> clientPool = new ConcurrentDictionary<string, HttpClient>();

        private HttpClient getClient(string urlString)
        {
            var url = new Uri(urlString);
            var key = $"{url.Host}:{url.Port}";
            if (!clientPool.ContainsKey(key))
            {
                clientPool[key] = createClient();
            }
            return clientPool[key];
        }

        private HttpClient createClient()
        {
            SocketsHttpHandler socketsHttpHandler = new SocketsHttpHandler();
            socketsHttpHandler.PooledConnectionLifetime = TimeSpan.FromMinutes(5);
            var client = new HttpClient(socketsHttpHandler);
            client.Timeout = TimeSpan.FromSeconds(3);
            try
            {
                client.DefaultRequestHeaders.Add("User-Agent", "FRCS/1.1");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
            }
            catch (Exception e)
            {
                Log.Error(e, "fail to add http client default header");
            }
            return client;
        }

        /// <summary>
        /// 读取HTTP Get响应包文。
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="uriString">资源地址</param>
        /// <param name="headers">可选的HTTP头列表</param>
        /// <returns>响应结果</returns>
        public T Get<T>(string uriString, Dictionary<string, string> headers = null)
        {
            T retv = default;

            HttpClient hc = getClient(uriString);
            var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }
            var resp = hc.Send(request);
            resp.EnsureSuccessStatusCode();

            var opt = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            opt.Converters.Add(new MyDateTimeConverter());
            var json = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            retv = JsonSerializer.Deserialize<T>(json, opt);

            return retv;
        }

        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        /// <summary>
        /// 读取HTTP POST结果。
        /// </summary>
        /// <typeparam name="T1">响应结果类型</typeparam>
        /// <typeparam name="T2">POST对象类型</typeparam>
        /// <param name="uriString">资源地址</param>
        /// <param name="data">请求包文对象</param>
        /// <param name="headers">可选HTTP头列表</param>
        /// <param name="logging">是否写入日志，默认为真</param>
        /// <returns>响应对象</returns>
        public T1 Post<T1, T2>(string uriString, T2 data, Dictionary<string, string> headers = null, bool logging = true)
        {
            if (logging)
                Log.Debug($"post to {uriString}\n{JsonSerializer.Serialize(data)}");
            T1 retv = default;

            HttpClient hc = getClient(uriString);
            var request = new HttpRequestMessage(HttpMethod.Post, uriString);
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }

            HttpContent content = new StringContent(JsonSerializer.Serialize(
                data,
                jsonOptions
            ));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            request.Content = content;
            var resp = hc.Send(request);
            resp.EnsureSuccessStatusCode();
            if (logging)
                Log.Debug(JsonSerializer.Serialize(resp));

            var opt = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            opt.Converters.Add(new MyDateTimeConverter());
            var json = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            if (logging)
                Log.Debug($"got post response\n{json}");
            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                Log.Warning($"post {uriString} return not ok\nStatusCode={resp.StatusCode.ToString()}\njson");
            retv = JsonSerializer.Deserialize<T1>(json, opt);

            return retv;
        }
    }
    public class MyDateTimeConverter : JsonConverter<DateTime>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }

}
