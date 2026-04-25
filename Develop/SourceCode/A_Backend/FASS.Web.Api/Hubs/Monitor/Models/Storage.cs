namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class Storage
    {
        public required string Code { get; set; }//库位编码
        public required string NodeCode { get; set; }//库位绑定编码
        public required string State { get; set; }//库位状态
        public bool IsLock { get; set; }//库位锁状态
    }
}
