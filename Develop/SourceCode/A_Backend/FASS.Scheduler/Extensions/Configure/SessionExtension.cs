using Common.NETCore.Models;
using FASS.Scheduler.Services;

namespace FASS.Scheduler.Extensions.Configure
{
    public static class SessionExtension
    {
        public static IServiceCollection AddCurrent(this IServiceCollection services)
        {
            Session.Current.User = () => new User { Id = AppHostService.EntryAssembly.Name };
            return services;
        }
    }
}
