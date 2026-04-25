namespace FASS.Service.Models.Report
{
    public class RequestParam
    {
        public string? CarCode { get; set; }
        public required string CreateAtStart { get; set; }
        public required string CreateAtEnd { get; set; }
        // public int day { get; set; }
    }
}
