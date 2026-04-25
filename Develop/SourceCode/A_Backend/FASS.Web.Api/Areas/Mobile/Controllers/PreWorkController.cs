using Common.NETCore;
using Common.NETCore.Extensions;
using Common.Service.Pager.Models;
using FASS.Service.Services.Warehouse.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FASS.Web.Api.Areas.Mobile.Controllers
{
    [Route("api/v1/Mobile/[controller]/[action]")]
    [Tags("移动管理-物料预定记录")]
    public class PreWorkController : BaseController
    {
        private readonly ILogger<PreWorkController> _logger;
        private readonly IStorageService _storageService;
        private readonly IPreWorkService _preWorkService;

        public PreWorkController(
            ILogger<PreWorkController> logger,
            IStorageService storageService,
            IPreWorkService preWorkService)
        {
            _logger = logger;
            _storageService = storageService;
            _preWorkService = preWorkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _preWorkService.ToPageAsync(param);
            var storageDtos = await _storageService.Set().Where(e => e.IsEnable).ToListAsync();
            for (var i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].SrcStorageName = storageDtos.Where(e => e.Id == page.Data[i].SrcStorageId).FirstOrDefault()?.Name;
                page.Data[i].DestStorageName = storageDtos.Where(e => e.Id == page.Data[i].DestStorageId).FirstOrDefault()?.Name;
            }
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
            var data = await _preWorkService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _preWorkService.DeleteAsync(keyValues);
            return Ok();
        }
    }
}
