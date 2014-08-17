using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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

        public class NamesViewModel {
            public string Email { get; set; }
            public string FullName { get; set; }
            public string UserName { get; set; }
        }

        [Route("names")]
        public async Task<NamesViewModel> GetUserNames() {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return new NamesViewModel {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
            };
        }


    }
}