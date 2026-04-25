using Common.Frame.Services.Trace.Interfaces;
using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Trace.Controllers
{
    [Route("api/v1/Trace/[controller]/[action]")]
    [Tags("系统日志-数据日志")]
    public class DataAuditController : BaseController
    {
        private readonly ILogger<DataAuditController> _logger;
        private readonly IDataAuditService _dataAuditService;

        public DataAuditController(
            ILogger<DataAuditController> logger,
            IDataAuditService dataAuditService)
        {
            _logger = logger;
            _dataAuditService = dataAuditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _dataAuditService.ToPageAsync(param);
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
            var data = await _dataAuditService.FirstOrDefaultAsync(keyValue);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _dataAuditService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM3()
        {
            await _dataAuditService.DeleteM3Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteM1()
        {
            await _dataAuditService.DeleteM1Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteW1()
        {
            await _dataAuditService.DeleteW1Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteD1()
        {
            await _dataAuditService.DeleteD1Async();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _dataAuditService.DeleteAllAsync();
            return Ok();
        }
    }
}