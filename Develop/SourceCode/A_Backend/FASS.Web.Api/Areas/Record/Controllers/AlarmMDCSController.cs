using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Service.Services.RecordExtend.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Common.NETCore;
using FASS.Service.Dtos.RecordExtend;
using OfficeOpenXml;


namespace FASS.Web.Api.Areas.Record.Controllers
{
    [Route("api/v1/Record/[controller]/[action]")]
    [Tags("业务记录-报警记录(Mdcs)")]
    public class AlarmMdcsController : BaseController
    {
        private readonly ILogger<AlarmMdcsController> _logger;
        private readonly IAlarmMdcsService _alarmService;

        public AlarmMdcsController(
            ILogger<AlarmMdcsController> logger,
            IAlarmMdcsService alarmService)
        {
            _logger = logger;
            _alarmService = alarmService;
        }

   

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            // 获取数据
            var dataInfo = await _alarmService.GetAlarmMdcsDtos();
            var data = dataInfo.ToList();

            // 创建 Excel 文件
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // 写入表头
            var headers = new[] { "CarCode", "CarName", "Name", "StartTime", "Continue" };
            for (int col = 0; col < headers.Length; col++)
            {
                worksheet.Cells[1, col + 1].Value = headers[col];
            }

            // 写入数据
            for (int row = 0; row < data.Count; row++)
            {
                worksheet.Cells[row + 2, 1].Value = data[row].CarCode;
                worksheet.Cells[row + 2, 2].Value = data[row].CarName;
                worksheet.Cells[row + 2, 3].Value = data[row].Name;
                worksheet.Cells[row + 2, 4].Value = data[row].StartTime;
                worksheet.Cells[row + 2, 5].Value = data[row].Continue;
            }

            // 设置文件名
            var fileName = $"ExportedData_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // 将 Excel 文件写入内存流
            using var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            // 返回文件
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _alarmService.ToPageAsync(param);
            for (int i = 0; i < page.Data.Count; i++)
            {
                var datediff = (page.Data[i].EndTime ?? DateTime.Now) - page.Data[i].StartTime;
                page.Data[i].Continue = string.Format(@"{0}小时{1}分{2}秒", Math.Floor(datediff.TotalHours), datediff.Minutes, datediff.Seconds);
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
            var data = await _alarmService.FirstOrDefaultAsync(e => e.Id == keyValue);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> ManualFinish([FromBody] List<string> keyValues)
        {
            await _alarmService.Repository.ExecuteUpdateAsync(e => keyValues.Contains(e.Id), s => s.SetProperty(b => b.IsFinish, true));
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<string> keyValues)
        {
            await _alarmService.DeleteAsync(keyValues);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM3()
        {
            await _alarmService.DeleteM3Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteM1()
        {
            await _alarmService.DeleteM1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteW1()
        {
            await _alarmService.DeleteW1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteD1()
        {
            await _alarmService.DeleteD1Async();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> DeleteAll()
        {
            await _alarmService.DeleteAllAsync();
            return Ok();
        }
    }
}
