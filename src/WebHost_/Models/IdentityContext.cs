using Microsoft.AspNet.Identity.EntityFramework;

namespace WebHost.Models {
    public class IdentityContext : IdentityDbContext<ApplicationUser> {
        public IdentityContext()
            : base("IdentityContext", throwIfV1Schema: false) {
        }

        public static IdentityContext Create() {
            return new IdentityContext();
        }
    }
}