using Common.NETCore.Extensions;
using Common.NETCore.Models;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace FASS.Web.Api.Extensions.Configure
{
    public static class ExceptionExtension
    {
        public static IApplicationBuilder UseException(this IApplicationBuilder app)
        {
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
                    context.Response.ContentType = Application.Json;
                    await context.Response.WriteAsJsonAsync(responseResult);
                });
            });
            //app.UseStatusCodePages(async context =>
            //{
            //    await Task.Run(() =>
            //    {
            //        if (context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            //        {
            //            context.HttpContext.Response.Redirect("/Login");
            //        }
            //        else if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound && context.HttpContext.Request.Path == "/index.html")
            //        {
            //            context.HttpContext.Response.Redirect("/Login");
            //        }
            //        else
            //        {
            //            context.HttpContext.Response.Redirect("/Error".AddQueryString(new { message = context.HttpContext.Response.StatusCode }));
            //        }
            //    });
            //});
            return app;
        }
    }
}