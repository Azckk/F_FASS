namespace FASS.Web.Api.Hubs.Monitor.Models
{
    public class StatusStatistics
    {
        public int OffLine { get; set; }
        public int Idle { get; set; }
        public int Tasking { get; set; }
        public int Charging { get; set; }
        public int Faulting { get; set; }
    }
}
