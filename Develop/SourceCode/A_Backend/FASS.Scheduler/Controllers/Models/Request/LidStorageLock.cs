namespace FASS.Scheduler.Controllers.Models.Request
{
    public class LidStorageLock
    {
        public required string AreaCode { get; set; }//库位所属编码
        //public string? StorageCode { get; set; }//库位编码
        public required string Operate { get; set; }//open、close
    }
}
