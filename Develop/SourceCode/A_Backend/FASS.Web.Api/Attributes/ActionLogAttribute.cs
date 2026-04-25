using Common.AspNetCore.Extensions;
using Common.AspNetCore.Helpers;
using Common.Frame.Dtos.Trace;
using Common.Frame.Services.Trace.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace FASS.Web.Api.Attributes
{
    public class ActionLogIgonreAttribute : ActionFilterAttribute
    {

    }
    public class ActionLogAttribute : ActionFilterAttribute
    {
        private readonly IUserActionService _userLogService;
        private readonly ILogger<ActionLogAttribute> _logger;

        public ActionLogAttribute(
            IUserActionService userLogService,
            ILogger<ActionLogAttribute> logger)
        {
            _userLogService = userLogService;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ignore = context.ActionDescriptor.FilterDescriptors.Select(f => f.Filter).OfType<TypeFilterAttribute>().Any(f => f.ImplementationType.Equals(typeof(ActionLogIgonreAttribute)));
            if (ignore == true)
            {
                return;
            }
            context.ActionDescriptor.Properties["Watch"] = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var ignore = context.ActionDescriptor.FilterDescriptors.Select(f => f.Filter).OfType<TypeFilterAttribute>().Any(f => f.ImplementationType.Equals(typeof(ActionLogIgonreAttribute)));
            if (ignore == true)
            {
                return;
            }
            var watch = context.ActionDescriptor.Properties["Watch"] as Stopwatch;
            var userActionDto = new UserActionDto
            {
                UserId = IdentityHelper.ToUserIdentity(context.HttpContext.User).Id,
                Controller = context.RouteData.DataTokens["area"] is null ? $"{context.RouteData.Values["controller"]}" : $"{context.RouteData.DataTokens["area"]}/{context.RouteData.Values["controller"]}",
                Action = $"{context.RouteData.Values["controller"]}",
                Watch = watch?.Elapsed.ToString(),
                RequestUrl = context.HttpContext.Request.GetAbsoluteUri(),
                RequestToken = context.HttpContext.Request.Cookies["Authorization"],
                ResponseCode = context.HttpContext.Response.StatusCode.ToString(),
                UserAgent = context.HttpContext.Request.Headers["User-Agent"],
                IpAddress = context.HttpContext.GetUserIp()
            };
            watch?.Stop();
            try
            {
                _userLogService.AddAsync(userActionDto).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }
        }
    }
}