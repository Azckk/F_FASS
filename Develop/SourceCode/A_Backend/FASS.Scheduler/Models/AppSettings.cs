using Common.Frame.Options;

namespace FASS.Scheduler.Models
{
    public class AppSettings
    {
        public App App { get; set; } = null!;
        public FrameOption Frame { get; set; } = null!;
        public Server Server { get; set; } = null!;
        public Scheduler Scheduler { get; set; } = null!;
        public Auth Auth { get; set; } = null!;
        public Plugin Plugin { get; set; } = null!;
        public Event Event { get; set; } = null!;
        public Extend Extend { get; set; } = null!;
        public Mdcs Mdcs { get; set; } = null!;
    }
    public class App
    {
        public required string ActivationCode { get; set; }
    }
    public class Server
    {
        public List<string> Urls { get; set; } = [];
        public string? TcpServerLocalIP { get; set; }
        public string? UdpServerLocalIP { get; set; }
    }
    public class Scheduler
    {
        public int LockCount { get; set; }
        public int ArchiveDueTime { get; set; }
        public int UpdateDueTime { get; set; }
        public int TrafficDueTime { get; set; }
        public int FlowDueTime { get; set; }
        public int CarDueTime { get; set; }
        public int DataDueTime { get; set; }
        public int StateDueTime { get; set; }
    }
    public class Auth
    {
        public required string SigningKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpireSeconds { get; set; }
    }
    public class Plugin
    {
        public bool EnablePlugin { get; set; }
        public string? PluginDirectory { get; set; }
    }
    public class Event
    {
        public bool EnableCar { get; set; }
        public string? CarConfig { get; set; }
        public bool EnableExtend { get; set; }
    }
    public class Extend
    {
        public bool EnableComClient { get; set; }
        public string? ComClientPortName { get; set; }
        public bool EnableTcpClient { get; set; }
        public string? TcpClientRemoteIP { get; set; }
        public bool EnableTcpServer { get; set; }
        public string? TcpServerLocalIP { get; set; }
        public bool EnableUdpServer { get; set; }
        public string? UdpServerRemoteIP { get; set; }
        public string? UdpServerLocalIP { get; set; }
        public bool EnableHttpClient { get; set; }
        public string? HttpClientBaseAddress { get; set; }
        public bool EnableHttpServer { get; set; }
        public List<string> HttpServerPrefixes { get; set; } = [];
        public bool EnableMdcsTaskSync { get; set; }
        public int MdcsTaskSyncDueTime { get; set; }
        public bool EnableStandardAlarmServer { get; set; }
        public int StandardAlarmDueTime { get; set; }
        public bool EnableEmptyBucketFilling { get; set; }
        public int FillingDueTime { get; set; }
    }
    public class Mdcs
    {
        public required string SimpleUrl { get; set; }
    }
}