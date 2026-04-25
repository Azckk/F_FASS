using Common.NETCore.Models;
using FASS.Web.Api.Services;

namespace FASS.Web.Api.Extensions.Configure
{
    public static class SessionExtension
    {
        public static IServiceCollection AddCurrent(this IServiceCollection services)
        {
            Session.Current.User = () => new User { Id = AppHostService.EntryAssembly.Name };
            return services;
        }

        public static IApplicationBuilder UseCurrent(this IApplicationBuilder app)
        {
            Session.Current.User = () => new User { Id = app.ApplicationServices.GetService<IHttpContextAccessor>()?.HttpContext?.User.FindFirst("id")?.Value ?? AppHostService.EntryAssembly.Name };
            return app;
        }
    }
}
