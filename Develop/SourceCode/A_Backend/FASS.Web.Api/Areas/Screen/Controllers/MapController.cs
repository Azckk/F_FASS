using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Areas.Screen.Controllers
{
    [Route("api/v1/Screen/[controller]/[action]")]
    [Tags("大屏管理-地图展示")]
    public class MapController : BaseController
    {
        private readonly ILogger<MapController> _logger;

        public MapController(
            ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}