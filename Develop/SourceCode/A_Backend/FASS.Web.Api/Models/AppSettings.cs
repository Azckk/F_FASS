using Common.Frame.Options;

namespace FASS.Web.Api.Models
{
    public class AppSettings
    {
        public App App { get; set; } = null!;
        public FrameOption Frame { get; set; } = null!;
        public Server Server { get; set; } = null!;
        public Auth Auth { get; set; } = null!;
        public Mdcs Mdcs { get; set; } = null!;
        public Scheduler Scheduler { get; set; } = null!;
    }

    public class App
    {
        public required string ActivationCode { get; set; }
    }

    public class Server
    {
        public List<string> Urls { get; set; } = [];
    }

    public class Auth
    {
        public required string SigningKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpireSeconds { get; set; }
    }

    public class Mdcs
    {
        public required string SimpleUrl { get; set; }
        public string? MapUrl { get; set; }
    }

    public class Scheduler
    {
        public string? SchedulerUrl { get; set; }
    }
}
