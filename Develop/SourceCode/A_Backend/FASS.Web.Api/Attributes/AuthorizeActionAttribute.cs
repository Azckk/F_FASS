using Common.AspNetCore.Helpers;
using Common.Frame.Services.Account.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FASS.Web.Api.Attributes
{
    public class AuthorizeActionIgonreAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
        }
    }
    public class AuthorizeActionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IPermissionService _permissionService;

        public AuthorizeActionAttribute(
            IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var ignore = context.ActionDescriptor.FilterDescriptors.Select(f => f.Filter).OfType<TypeFilterAttribute>().Any(f => f.ImplementationType.Equals(typeof(AuthorizeActionIgonreAttribute)));
            if (ignore)
            {
                return;
            }
            var userIdentity = IdentityHelper.ToUserIdentity(context.HttpContext.User);
            if (userIdentity.IsSystem)
            {
                return;
            }
            //var target = context.HttpContext.Request.Path;
            //var isOk = _permissionService.CheckTargetAsync(userIdentity.Id, target).Result;
            var isOk = true;
            if (isOk)
            {
                return;
            }
            context.Result = new UnauthorizedResult();
        }
    }
}