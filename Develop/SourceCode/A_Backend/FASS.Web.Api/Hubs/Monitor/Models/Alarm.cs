namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Alarm
    {
        public string? Level { get; set; }
        public string? Type { get; set; }
        public string? Code { get; set; }//车辆编码
        public string? Name { get; set; }//车辆名称
        public string? Message { get; set; }//报警描述
        public string? Data { get; set; }
        public string? State { get; set; }//报警编码
        public DateTime StartTime { get; set; }//报警开始时间
    }
}
