using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebHost.Models;

namespace WebHost.Controllers {
    [Authorize]
    [RoutePrefix("account")]
    public class AccountApiController : ApiController {
        private ApplicationUserManager _userManager;

        public AccountApiController() {
        }

        public AccountApiController(ApplicationUserManager userManager) {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        [Route("names")]
        [AllowAnonymous]
        public async Task<NamesViewModel> GetUserNames() {
            if (!User.Identity.IsAuthenticated) { return null; }
            var ci = (ClaimsIdentity) User.Identity;
            return new NamesViewModel {
                Email = ci.FindFirstValue(ClaimNames.Email),
                UserName = ci.FindFirstValue(ClaimNames.Username),
                FullName = ci.FindFirstValue(ClaimNames.FullName)
            };
        }


    }
}