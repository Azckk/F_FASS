namespace FASS.Service.Models.Set
{
    public class ConfigTrafficControlSet
    {
        public bool Search_AllowDestOnRoute { get; set; }
        public int Search_RouteOnDestClearance { get; set; }
        public int Search_MaxVisitPerSite { get; set; }
        public int Search_MaxDupVisit { get; set; }
        public int Search_MaxKeepResult { get; set; }
        public int Traffic_MaxHoldingLocks { get; set; }
        public int Traffic_DeadLockSearchDepth { get; set; }
        public bool Auto_Reprogram { get; set; }
        public string MapUrl { get; set; }
    }
}
