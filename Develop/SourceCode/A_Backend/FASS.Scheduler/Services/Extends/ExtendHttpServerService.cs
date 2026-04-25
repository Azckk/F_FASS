using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Boot.Services;
using FASS.Scheduler.Services.Cars.Fairyland.Pc.Extensions;
using System.Net;
using System.Text;
using HttpServer = Common.Net.Http.HttpServer;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendHttpServerService
    {
        public ExtendService ExtendService { get; }

        public IBootService BootService { get; }

        public bool IsStarted { get; private set; }

        public HttpServer HttpServer { get; private set; } = null!;

        public ExtendHttpServerService(
            ExtendService extendService)
        {
            ExtendService = extendService;

            BootService = ExtendService.ServiceProvider.GetRequiredService<IBootService>();

            Init();
        }

        public void Init()
        {
            HttpServer = new HttpServer();
            HttpServer.Prefixes = ExtendService.AppSettings.Extend.HttpServerPrefixes;
            HttpServer.Started += (httpServer) =>
            {
                ExtendService.Logger.LogInformation($"Started [{string.Join(',', httpServer.Server.Prefixes.Select(e => e))}]");
            };
            HttpServer.Stopped += (httpServer) =>
            {
                ExtendService.Logger.LogInformation($"Stopped");
                httpServer.Started = null;
                Thread.Sleep(3000);
                httpServer.Start();
            };
            HttpServer.Method += (httpServer, context, requestContent) =>
            {
                ExtendService.Logger.LogInformation($"HttpMethod [{context.Request.HttpMethod}] Request [{context.Request.RawUrl}] RequestContent \n{requestContent}");
            };
            HttpServer.Get += (httpServer, context, requestContent) =>
            {
                var responseResult = new ResponseResult();
                responseResult.Success = true;
                responseResult.Data = requestContent;
                SendResponse(context, responseResult);
                return;
            };
            HttpServer.Post += (httpServer, context, requestContent) =>
            {
                var responseResult = new ResponseResult();
                try
                {
                    if (context.Request.RawUrl?.Equals("/agv/carState", StringComparison.OrdinalIgnoreCase) ?? false)
                    {
                        var requestJson = requestContent.JsonParse();
                        if (!requestJson.RootElement.TryGetProperty("carCode", out var carCodeJson))
                        {
                            responseResult.Success = false;
                            responseResult.Message = "获取参数失败 [carCode]";
                            SendResponse(context, responseResult);
                            return;
                        }
                        var carCode = carCodeJson.GetString();
                        if (string.IsNullOrWhiteSpace(carCode))
                        {
                            var cars = BootService.Cars.ToList();
                            responseResult.Success = true;
                            responseResult.Data = cars.Select(e => e.ToCarState()).ToList();
                            SendResponse(context, responseResult);
                            return;
                        }
                        var car = BootService.Cars.FirstOrDefault(e => e.Code == carCode);
                        if (car == null)
                        {
                            responseResult.Success = false;
                            responseResult.Message = $"获取车辆失败 [{carCode}]";
                            SendResponse(context, responseResult);
                            return;
                        }
                        responseResult.Success = true;
                        responseResult.Data = car.ToCarState();
                        SendResponse(context, responseResult);
                        return;
                    }
                    else
                    {
                        responseResult.Success = false;
                        responseResult.Message = "无效接口";
                        SendResponse(context, responseResult);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    responseResult.Success = false;
                    responseResult.Message = ex.Message;
                    SendResponse(context, responseResult);
                    return;
                }
            };
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
                HttpServer.StartAndAccept();
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
                if (IsStarted)
                {
                    return;
                }
                IsStarted = true;
                HttpServer.Stop();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void SendResponse(HttpListenerContext context, ResponseResult responseResult)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json;charset=UTF-8";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(responseResult.ToJson()));
            ExtendService.Logger.LogInformation($"HttpServer HttpMethod [{context.Request.HttpMethod}] Request [{context.Request.RawUrl}] ResponseResult \n{responseResult.ToJson()}");
        }
    }
}