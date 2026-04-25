using Common.NETCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FASS.Web.Api.Attributes
{
    public class ResultAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is StatusCodeResult statusCodeResult)
            {
                var responseResult = new ResponseResult();
                responseResult.Code = statusCodeResult.StatusCode;
                if (statusCodeResult is OkResult)
                {
                    responseResult.Success = true;
                    responseResult.Data = Enum.GetName(typeof(HttpStatusCode), (HttpStatusCode)statusCodeResult.StatusCode);
                }
                else
                {
                    responseResult.Success = false;
                    responseResult.Message = Enum.GetName(typeof(HttpStatusCode), (HttpStatusCode)statusCodeResult.StatusCode);
                }
                context.Result = new OkObjectResult(responseResult);
            }
            else if (context.Result is ObjectResult objectResult)
            {
                var responseResult = new ResponseResult();
                responseResult.Code = objectResult?.StatusCode ?? 0;
                if (objectResult is OkObjectResult)
                {
                    responseResult.Success = true;
                    responseResult.Data = objectResult.Value;
                }
                else
                {
                    responseResult.Success = false;
                    responseResult.Message = objectResult?.Value?.ToString();
                }
                context.Result = new OkObjectResult(responseResult);
            }
        }
    }
}