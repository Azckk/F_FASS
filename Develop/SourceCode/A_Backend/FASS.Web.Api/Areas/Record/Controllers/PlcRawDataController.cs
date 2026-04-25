using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Services.Record.Interfaces;
using FASS.Service.Consts.Record;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;

namespace FASS.Web.Api.Areas.Record.Controllers
{
    [Route("api/v1/Record/[controller]/[action]")]
    [Tags("业务记录-任务呼叫记录")]
    public class PlcRawDataController : BaseController
    {
        private readonly ILogger<PlcRawDataController> _logger;
        private readonly IDiaryService _diaryService;

        public PlcRawDataController(
            ILogger<PlcRawDataController> logger,
            IDiaryService diaryService)
        {
            _logger = logger;
            _diaryService = diaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            param.Where.Add(new Where()
            {
                Logic = WhereLogic.And,
                Field = "type",
                Operator = WhereOperator.Equal,
                Value = DiaryExtendConst.Type.PlcMessage
            });
            var page = await _diaryService.ToPageAsync(param);
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
            var data = await _diaryService.FirstOrDefaultAsync(e => e.Id == keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _diaryService.DeleteM3Async(type: "PlcMessage");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _diaryService.DeleteM1Async(type: "PlcMessage");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _diaryService.DeleteW1Async(type: "PlcMessage");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _diaryService.DeleteD1Async(type: "PlcMessage");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _diaryService.DeleteAllAsync(type: "PlcMessage");
            return Ok();
        }

    }
}
