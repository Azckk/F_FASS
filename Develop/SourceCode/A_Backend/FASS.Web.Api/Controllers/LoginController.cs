using Common.AspNetCore.Helpers;
using Common.Frame.Dtos.Account;
using Common.Frame.Models.Account;
using Common.Frame.Services.Account.Interfaces;
using Common.NETCore;
using Common.NETCore.Helpers;
using FASS.Web.Api.Extensions;
using FASS.Web.Api.Models;
using FASS.Web.Api.Services;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    [Tags("系统授权")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IDistributedCache _distributedCache;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public LoginController(
            ILogger<LoginController> logger,
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
        public IActionResult AppName()
        {
            return Ok(new
            {
                AppName = $"{AppHostService.EntryAssembly.Name} V {AppHostService.EntryAssembly.Version}"
            });
        }

        [HttpGet]
        public IActionResult Captcha()
        {
            var captcha = Utility.Captcha.Create(_distributedCache, CacheKey.Login.Captcha);
            return Ok(new
            {
                CacheKey = captcha.Item1,
                CaptchaImg = $"data:image/png;base64,{Convert.ToBase64String(captcha.Item2)}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin.EnableCaptcha)
            {
                if (!Utility.Captcha.Check(_distributedCache, Guard.NotNull(userLogin.CacheKey), Guard.NotNull(userLogin.CaptchaCode)))
                {
                    return BadRequest("验证码错误");
                }
            }
            var userDto = await _userService.GetEntityByUsernameAsync(userLogin.Username);
            if (userDto == null)
            {
                return BadRequest("用户不存在");
            }
            if (!userDto.IsEnable)
            {
                return BadRequest("用户被禁用");
            }
            if (userDto.Password != SecurityHelper.HashSHA256(userLogin.Password))
            {
                return BadRequest("用户名或密码错误");
            }
            var accessToken = _appSettings.GetToken(_userService.ToClaims(userDto));
            var refreshToken = _appSettings.RefreshToken(accessToken);
            var data = new
            {
                Username = userDto.Username,
                Roles = "",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Eexpires = DateTime.Now
            };
            return Ok(data);
        }

        [HttpGet]
        public IActionResult RefreshToken()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var accessToken = _appSettings.GetToken(IdentityHelper.ToClaims(userIdentity));
            var refreshToken = _appSettings.RefreshToken(accessToken);
            var data = new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Eexpires = DateTime.Now
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsyncRoutes()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var dtos = await _permissionService.GetListByUserIdAsync(userIdentity.Id);
            var data = GetPermission(dtos, "Root");
            return Ok(data);
        }

        private IEnumerable<dynamic> GetPermission(IEnumerable<PermissionDto> permissions, string parentId)
        {
            var result = permissions.Where(e => e.ParentId == parentId).Select(e =>
            {
                var children = GetPermission(permissions, e.Id);
                return new
                {
                    Path = e.Target?.ToLowerInvariant(),
                    Name = e.Code,
                    Meta = new
                    {
                        Icon = Icon.ToIcon(e.Icon),
                        Title = e.Name,
                        Code = e.Code,
                        KeepAlive = e.Code.Contains("map", StringComparison.OrdinalIgnoreCase) || e.Code.Contains("runtime", StringComparison.OrdinalIgnoreCase),//地图相关页面开启缓存功能
                        ShowParent = true
                    },
                    Children = children.Any() ? children : null
                };
            });
            return result.ToList();
        }
    }
}