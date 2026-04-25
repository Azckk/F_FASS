using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Dtos.Data;
using FASS.Data.Services.Data.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Data.Controllers
{
    [Route("api/v1/Data/[controller]/[action]")]
    [Tags("数据管理-避让点管理")]
    public class AvoidController : BaseController
    {
        private readonly ILogger<AvoidController> _logger;
        private readonly IAvoidService _avoidService;

        public AvoidController(
            ILogger<AvoidController> logger,
            IAvoidService avoidService)
        {
            _logger = logger;
            _avoidService = avoidService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _avoidService.ToPageAsync(param);
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
            var data = await _avoidService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> AddOrUpdate([FromQuery] string? keyValue, [FromBody] AvoidDto avoidDto)
        {
            await _avoidService.AddOrUpdateAsync(keyValue, avoidDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _avoidService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetListToSelect()
        {
            var data = await _avoidService.Set().Where(e => e.IsEnable).OrderByDescending(e => e.CreateAt).ToListAsync();
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Select()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> SelectGetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _avoidService.SelectGetPageAsync(param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        //[HttpGet]
        //public IActionResult Car()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> CarGetPage([FromQuery] string keyValue, [FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _avoidService.CarGetPageAsync(keyValue, param);
            var data = new
            {
                rows = page.Data,
                total = page.TotalCount
            };
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> CarAdd([FromQuery] string keyValue, [FromBody] List<string> carIds)
        {
            await _avoidService.CarAddAsync(keyValue, carIds);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> CarDelete([FromQuery] string keyValue, [FromBody] List<string> carIds)
        {
            await _avoidService.CarDeleteAsync(keyValue, carIds);
            return Ok();
        }
    }
}