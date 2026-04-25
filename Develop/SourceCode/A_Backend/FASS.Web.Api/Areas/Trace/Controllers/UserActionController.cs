using Common.Frame.Services.Trace.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Trace.Controllers
{
    [Route("api/v1/Trace/[controller]/[action]")]
    [Tags("系统日志-用户日志")]
    public class UserActionController : BaseController
    {
        private readonly ILogger<UserActionController> _logger;
        private readonly IUserActionService _userActionService;

        public UserActionController(
            ILogger<UserActionController> logger,
            IUserActionService userActionService)
        {
            _logger = logger;
            _userActionService = userActionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _userActionService.ToPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _userActionService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _userActionService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM3()
        {
            await _userActionService.DeleteM3Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM1()
        {
            await _userActionService.DeleteM1Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteW1()
        {
            await _userActionService.DeleteW1Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteD1()
        {
            await _userActionService.DeleteD1Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _userActionService.DeleteAllAsync();
            return Ok();
        }
    }
}