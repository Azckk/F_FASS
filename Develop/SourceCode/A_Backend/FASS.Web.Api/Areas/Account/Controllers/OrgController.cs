using Common.Frame.Dtos.Account;
using Common.Frame.Services.Account.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Account.Controllers
{
    [Route("api/v1/Account/[controller]/[action]")]
    [Tags("系统权限-组织管理")]
    public class OrgController : BaseController
    {
        private readonly ILogger<OrgController> _logger;
        private readonly IOrgService _orgService;

        public OrgController(
            ILogger<OrgController> logger,
            IOrgService orgService)
        {
            _logger = logger;
            _orgService = orgService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _orgService.ToPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var data = await _orgService.ToListAsync(e => true);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _orgService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] OrgDto orgDto)
        {
            await _orgService.AddOrUpdateAsync(keyValue, orgDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            var ids = await _orgService.GetChildrenIdsAsync(keyValues);
            await _orgService.DeleteAsync(ids);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _orgService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _orgService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTree()
        {
            var data = await _orgService.GetTreeAsync();
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Org()
        //{
        //    return View();
        //}
    }
}