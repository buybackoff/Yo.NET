using Microsoft.AspNet.Identity.EntityFramework;

namespace Yo.WebHost {
    public class ApplicationUser : IdentityUser {
        public string CustomUserProperty { get; set; }
    }
}