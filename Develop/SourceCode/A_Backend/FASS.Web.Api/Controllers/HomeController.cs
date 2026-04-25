using Common.AspNetCore.Extensions;
using Common.AspNetCore.Helpers;
using Common.Frame.Dtos.Frame;
using Common.Frame.Services.Account.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Controllers
{
    [Tags("系统主页")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IDistributedCache _distributedCache;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public HomeController(
            ILogger<HomeController> logger,
            AppSettings appSettings,
            IDistributedCache distributedCache,
            IUserService userService,
            IPermissionService permissionService)
        {
            _logger = logger;
            _appSettings = appSettings;
            _distributedCache = distributedCache;
            _userService = userService;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var userDto = await _userService.FirstOrDefaultAsync(userIdentity.Id);
            if (userDto is null)
            {
                return Logout();
            }
            var data = new
            {
                id = userIdentity.Id,
                username = userIdentity.Username,
                name = userIdentity.Name,
                system = userIdentity.IsSystem,
                avatar = userDto.AvatarSrc
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPermission()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var data = await _permissionService.GetTreeByUserIdAsync(userIdentity.Id);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDictItem()
        {
            var data = await _distributedCache.GetAsync<List<DictItemDto>>(CacheKey.Setting.DictItem);
            if (data is not null)
            {
                data = data.OrderBy(e => e.SortNumber).ToList();
            }
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetHome()
        {
            var data = await _permissionService.FirstOrDefaultAsync(e => e.Target == "/Monitor/Runtime/Index");
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Headers.Remove("Authorization");
            return Ok();
        }

        [HttpGet]
        public IActionResult GetActivationCode()
        {
            return Ok(new
            {
                ActivationCode = $"{_appSettings.App.ActivationCode}"
            });
        }
    }
}