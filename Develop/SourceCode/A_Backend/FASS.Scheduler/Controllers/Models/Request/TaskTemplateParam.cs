namespace FASS.Scheduler.Controllers.Models.Request
{
    public class TaskTemplateParam
    {
        public required string CarCode { get; set; }
        public required string TaskTemplateCode { get; set; }
        public string? TaskTemplateNodeCodes { get; set; }
        public string? TaskTemplateEdgeCodes { get; set; }
    }
}