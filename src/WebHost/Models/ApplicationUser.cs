using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebHost.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public static class ClaimNames {
        public const string Email = "urn:yo:email";
        public const string EmailConfirmed = "urn:yo:emailconfirmed";
        public const string FullName = "urn:yo:fullname";
        public const string Username = "urn:yo:username";
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