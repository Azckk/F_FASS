using Common.NETCore.Models;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Models;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FASS.Web.Api.Areas.Signal.Controllers
{

    [Route("api/v1/signal/[controller]/[action]")]
    [Tags("安全信号")]
    public class GeneralSafetySignalController : BaseController
    {
        private readonly ILogger<GeneralSafetySignalController> _logger;
        private readonly AppSettings _appSettings;

        public GeneralSafetySignalController(
            ILogger<GeneralSafetySignalController> logger,
            AppSettings appSettings
        )
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult GetSafetySignals()
        {
            try
            {
                var resp = WebAPIHelper.Instance.Get<ResponseResult>(
                    $"{_appSettings.Mdcs.SimpleUrl}/mission/get_mission_status_sls"
                );
                if (resp.Code == 200)
                {
                    return Ok(resp.Data);
                }
                else
                {
                    return BadRequest(resp.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"获取simple安全信号失败：{ex.Message}");
            }
        }

    }

}
