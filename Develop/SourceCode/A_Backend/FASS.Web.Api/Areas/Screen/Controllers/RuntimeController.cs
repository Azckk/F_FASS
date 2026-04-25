using FASS.Data.Services.Base.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FASS.Web.Api.Areas.Screen.Controllers
{
    [Route("api/v1/Screen/[controller]/[action]")]
    [Tags("大屏管理-运行展示")]
    public class RuntimeController : BaseController
    {
        private readonly ILogger<RuntimeController> _logger;
        private readonly IMapService _mapService;

        public RuntimeController(
            ILogger<RuntimeController> logger,
            IMapService mapService)
        {
            _logger = logger;
            _mapService = mapService;
        }

        //public override IActionResult Index()
        //{
        //    ViewBag.ServiceApi = "/Monitor/RuntimeHub";
        //    return base.Index();
        //}

        [HttpGet]
        public async Task<IActionResult> GetMap()
        {
            var data = new
            {
                map = await _mapService.Set().FirstOrDefaultAsync()
            };
            return Ok(data);
        }
    }
}