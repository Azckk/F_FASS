namespace FASS.Scheduler.Controllers.Models.Request
{
    public class StorageTag
    {
        public string? StorageCode { get; set; }//库位编号
        public required string NodeCode { get; set; }//库位对应站点编码
        public required string Operate { get; set; }//open、close
    }
}
