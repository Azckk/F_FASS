using Common.Service.Pager.Models;
using Common.NETCore.Extensions;
using FASS.Data.Services.Data.Interfaces;
using FASS.Data.Services.Record.Interfaces;
using FASS.Web.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.NETCore;
using FASS.Data.Services.Record;
using OfficeOpenXml;
using AutoMapper;
using Common.EntityFramework.Services;
using FASS.Service.Dtos.RecordExtend;

namespace FASS.Web.Api.Areas.Monitor.Controllers
{
    [Route("api/v1/Monitor/[controller]/[action]")]
    [Tags("监控管理-报警监控")]
    public class AlarmController : BaseController
    {
        private readonly ILogger<AlarmController> _logger;
        private readonly IAlarmService _alarmService;
        private readonly ICarService _carService;

        public AlarmController(
            ILogger<AlarmController> logger,
            IAlarmService alarmService,
            ICarService carService)
        {
            _logger = logger;
            _alarmService = alarmService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] string pageParam)
        {
            var param = Guard.NotNull(pageParam.JsonTo<Page>());
            var page = await _alarmService.ToPageAsync(param);
            var carDtos = await _carService.Set().ToListAsync();
            for (int i = 0; i < page.Data.Count; i++)
            {
                page.Data[i].Remark = carDtos.Where(e => e.Code == page.Data[i].Code).FirstOrDefault()?.Name;
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

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            // 获取数据
            //var dataInfo = await _alarmService.Set().Where(c=>);
            // 获取当前时间的一个月前
            // var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var oneMonthAgo = DateTime.Now.AddDays(-15);

            // 查询最近一个月的数据
            var data = await _alarmService
                .Set()
                .Where(e =>
                    e.CreateAt >= oneMonthAgo // 筛选条件：CreateAt 在一个月内
                )

                .ToListAsync();
            //var Dtos = Mapper.Map<IEnumerable<AlarmMdcsDto>>(Entities);
           // var data = Dtos.ToList();

            // 创建 Excel 文件
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // 写入表头
            var headers = new[] { "Remark", "Message", "CreateAt"};
            for (int col = 0; col < headers.Length; col++)
            {
                worksheet.Cells[1, col + 1].Value = headers[col];
            }

            // 写入数据
            for (int row = 0; row < data.Count; row++)
            {
                worksheet.Cells[row + 2, 1].Value = data[row].Remark;
                worksheet.Cells[row + 2, 2].Value = data[row].Message;
                worksheet.Cells[row + 2, 3].Value = data[row].CreateAt;
               /* worksheet.Cells[row + 2, 4].Value = data[row].StartTime;
                worksheet.Cells[row + 2, 5].Value = data[row].Continue;*/
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
    }
}