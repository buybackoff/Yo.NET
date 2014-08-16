using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebHost.Models;

namespace WebHost.Providers {
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId) {
            if (publicClientId == null) {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context) {
            if (context.ClientId == _publicClientId) {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri) {
                    context.Validated();
                } else if (context.ClientId == "web") {
                    var expectedUri = new Uri(context.Request.Uri, "/");
                    context.Validated(expectedUri.AbsoluteUri);
                }
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret)) {
                if (clientId == "web" && clientSecret == "web") {
                    context.Validated();
                } 
            }
            return Task.FromResult(0);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null) {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public static AuthenticationProperties CreateProperties(string userName) {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}