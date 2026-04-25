using Common.Frame.Dtos.Account;
using Common.Frame.Services.Account.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Web.Api.Controllers.Base;
using FASS.Web.Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.AspNetCore.Helpers;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Account.Controllers
{
    [Route("api/v1/Account/[controller]/[action]")]
    [Tags("系统权限-权限管理")]
    public class PermissionController : BaseController
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IPermissionService _permissionService;

        public PermissionController(
            ILogger<PermissionController> logger,
            IPermissionService permissionService)
        {
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _permissionService.ToPageAsync(param);
            var data = new
            {
                rows = Icon.ToIcon(page.Data).ToList(),
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var dtos = await _permissionService.Set().OrderBy(e => e.SortNumber).ToListAsync();
            dtos = Icon.ToIcon(dtos).ToList();
            return Ok(dtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetEntity([FromQuery] string keyValue)
        {
            var data = await _permissionService.FirstOrDefaultAsync(keyValue);
            if (data == null)
            {
                return BadRequest();
            }
            data.Icon = Icon.ToIcon(data?.Icon ?? "");
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] PermissionDto permissionDto)
        {
            await _permissionService.AddOrUpdateAsync(keyValue, permissionDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            var ids = await _permissionService.GetChildrenIdsAsync(keyValues);
            await _permissionService.DeleteAsync(ids);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _permissionService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _permissionService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTree()
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var data = await _permissionService.GetTreeByUserIdAsync(userIdentity.Id);
            data = Icon.ToIcon(data).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTarget(string target)
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var data = await _permissionService.GetTargetAsync(userIdentity.Id, target);
            data = Icon.ToIcon(data).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> CheckTarget(string target)
        {
            var userIdentity = IdentityHelper.ToUserIdentity(User);
            var data = await _permissionService.CheckTargetAsync(userIdentity.Id, target);
            return Ok(data);
        }
    }
}