namespace FASS.Scheduler.Controllers.Models.Response
{
    public class StorageLockInfo
    {
        public required string NodeCode { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? AreaCode { get; set; }
        public required string Operate { get; set; }
    }
}
