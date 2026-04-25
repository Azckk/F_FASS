namespace FASS.Scheduler.Controllers.Models.Request
{
    public class TaskExchange
    {
        public string? CarCode { get; set; }
        public string? CarType { get; set; }
        public required string StorageCode { get; set; }//叫料工位
        public string CallMode { get; set; } = "FullEmptyExchange"; //叫料模式
    }
}
