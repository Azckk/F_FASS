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
    [Tags("系统权限-角色管理")]
    public class RoleController : BaseController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(
            ILogger<RoleController> logger,
            IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _roleService.ToPageAsync(param);
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
            var data = await _roleService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] RoleDto roleDto)
        {
            await _roleService.AddOrUpdateAsync(keyValue, roleDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _roleService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _roleService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _roleService.DisableAsync(keyValues);
            return Ok();
        }

        //[HttpGet]
        //public new IActionResult User()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> UserGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _roleService.UserGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> UserAdd([FromQuery] string keyValue, [FromBody] List<string> userIds)
        {
            await _roleService.UserAddAsync(keyValue, userIds);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> UserDelete([FromQuery] string keyValue, [FromBody] List<string> userIds)
        {
            await _roleService.UserDeleteAsync(keyValue, userIds);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Permission()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> PermissionGetTree([FromQuery] string keyValue)
        {
            var data = await _roleService.PermissionGetTreeAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> PermissionUpdate([FromQuery] string keyValue, [FromBody] List<string> permissionIds)
        {
            await _roleService.PermissionUpdateAsync(keyValue, permissionIds);
            return Ok();
        }
    }
}