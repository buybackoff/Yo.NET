using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebHost.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public static class ClaimNames {
        private static string id = WebConfigurationManager.AppSettings["ApplicationUrnKey"] ?? "my";
        static ClaimNames() {
            
        }
        public static string Email = "urn:" + id + ":email";
        public static string EmailConfirmed = "urn:" + id + ":emailconfirmed";
        public static string FullName = "urn:" + id + ":fullname";
        public static string Username = "urn:" + id + ":username";
    }
    
    public class ApplicationUser : IdentityUser {
        
        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            return await GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimNames.Email, Email));
            if (EmailConfirmed) {
                userIdentity.AddClaim(new Claim(ClaimNames.EmailConfirmed, "1"));
            }
            userIdentity.AddClaim(new Claim(ClaimNames.FullName, FullName));
            userIdentity.AddClaim(new Claim(ClaimNames.Username, UserName));
            return userIdentity;
        }
    }
}