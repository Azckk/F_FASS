using Microsoft.AspNetCore.Identity;

namespace FASS.Scheduler.Models
{
    public class UserIdentity : IdentityUser
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public bool IsSystem { get; set; }
        public string? Time { get; set; }
    }
}
