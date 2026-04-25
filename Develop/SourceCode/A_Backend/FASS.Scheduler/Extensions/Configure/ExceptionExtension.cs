using Common.NETCore.Extensions;
using Common.NETCore.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace FASS.Scheduler.Extensions.Configure
{
    public static class ExceptionExtension
    {
        public static IApplicationBuilder UseException(this IApplicationBuilder app)
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error.GetBaseException();
                    var responseResult = new ResponseResult()
                    {
                        Success = false,
                        Code = context.Response.StatusCode
                    };
                    if (ex != null)
                    {
                        responseResult.Message = ex.Message;
                    }
                    else
                    {
                        responseResult.Message = "未知错误";
                    }
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.Body.WriteAsync(responseResult.ToJson(jsonSerializerOptions).ToBytes());
                });
            });
            return app;
        }
    }
}