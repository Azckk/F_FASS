namespace FASS.Service.Models.Report
{
    public class AlarmReportData
    {
        public long AlarmCount { get; set; }
        public string? Name { get; set; }
        public required string Code { get; set; }
    }
}
