using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-首页菜单")]
    public class MenuController : BaseController
    {
        private readonly ILogger<MenuController> _logger;

        public MenuController(
            ILogger<MenuController> logger)
        {
            _logger = logger;
        }
    }
}