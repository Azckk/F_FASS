namespace FASS.Scheduler.Controllers.Models.Response
{
    public class StorageState
    {
        public string? NodeCode { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public required string State { get; set; }
        public string? AreaName { get; set; }
    }
}
