using Common.AspNetCore.Helpers;
using Common.Frame.Services.Account.Interfaces;
using Common.Frame.Services.Frame.Interfaces;
using FASS.Data.Services.Setting.Interfaces;
using FASS.Service.Services.Setting.Interfaces;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-移动首页")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppSettings _appSettings;

        private readonly IDistributedCache _distributedCache;
        private readonly IConfigDataService _configDataService;
        private readonly IConfigServiceService _configServiceService;
        private readonly IConfigService _configService;
        private readonly IDictItemService _dictItemService;

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public HomeController(
            ILogger<HomeController> logger,
            AppSettings appSettings,
            IDistributedCache distributedCache,
            IConfigDataService configDataService,
            IConfigServiceService configServiceService,
            IConfigService configService,
            IDictItemService dictItemService,
            IUserService userService,
            IPermissionService permissionService)
        {
            _logger = logger;
            _appSettings = appSettings;
            _distributedCache = distributedCache;
            _configDataService = configDataService;
            _configServiceService = configServiceService;
            _configService = configService;
            _dictItemService = dictItemService;
            _userService = userService;
            _permissionService = permissionService;
        }

        //[TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
        //public override IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var userDto = await _userService.FirstOrDefaultAsync(userIdentity.Id);
            if (userDto == null)
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
            var data = await _dictItemService.ToListAsync(e => e.IsEnable);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetHome()
        {
            var data = await _permissionService.FirstOrDefaultAsync(e => e.Target == "/Mobile/Home/Index");
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Authorization");
            return Ok();
        }
    }
}