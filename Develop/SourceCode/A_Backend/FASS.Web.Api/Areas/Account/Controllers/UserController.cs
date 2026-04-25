using Common.Frame.Dtos.Account;
using Common.Frame.Models.Account;
using Common.Frame.Services.Account.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Account.Controllers
{
    [Route("api/v1/Account/[controller]/[action]")]
    [Tags("系统权限-用户管理")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _userService.ToPageAsync(param);
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
            var data = await _userService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                if (string.IsNullOrWhiteSpace(userDto.Username))
                {
                    return BadRequest($"用户 [ {userDto.Username} ] 空值");
                }
                if (await _userService.ExistUsernameAsync(userDto.Username))
                {
                    return BadRequest($"用户 [ {userDto.Username} ] 已存在");
                }
            }
            userDto.Password = SecurityHelper.HashSHA256("123456");
            await _userService.AddOrUpdateAsync(keyValue, userDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _userService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Enable([FromBody] List<string> keyValues)
        {
            await _userService.EnableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Disable([FromBody] List<string> keyValues)
        {
            await _userService.DisableAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ResetPassword([FromBody] List<string> keyValues)
        {
            await _userService.PasswordAsync(keyValues, SecurityHelper.HashSHA256("123456"));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetEntityByUsername([FromQuery] string username)
        {
            var data = await _userService.GetEntityByUsernameAsync(username);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> ExistUsername([FromQuery] string username)
        {
            var data = await _userService.ExistUsernameAsync(username);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword([FromQuery] string keyValue, [FromBody] UserPassword userPassword)
        {
            var userDto = await _userService.FirstOrDefaultAsync(keyValue);
            if (userDto?.Password != SecurityHelper.HashSHA256(userPassword.PasswordOrigin))
            {
                return BadRequest("原密码错误");
            }
            await _userService.PasswordAsync(keyValue, SecurityHelper.HashSHA256(userPassword.PasswordNew));
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Provider()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult Password()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult Select()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SelectGetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _userService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }
    }
}