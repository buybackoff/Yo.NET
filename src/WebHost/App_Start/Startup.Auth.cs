using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Owin.Security.Providers.LinkedIn;
using Owin.Security.Providers.Yahoo;
using WebHost.Models;
using WebHost.Providers;

namespace WebHost {
    public partial class Startup {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup() {
            PublicClientId = "web";
            OAuthOptions = new OAuthAuthorizationServerOptions {
                TokenEndpointPath = new PathString("/account/token"),
                AuthorizeEndpointPath = new PathString("/account/authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                AllowInsecureHttp = true
            };
        }
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }


        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app) {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(IdentityContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromDays(7),
                SlidingExpiration = true,
                CookieHttpOnly = true,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");
            var twitterOptions = new TwitterAuthenticationOptions {
                ConsumerKey = "jtN9dYYy2r4UqKBSo3trTLUqL",
                ConsumerSecret = "9octzU33bA6y5TefZEIg6z5fp9CreVrdgS5tT3E60qpQ399EjM",
                CallbackPath = new PathString("/account/signin-twitter")
            };
            app.UseTwitterAuthentication(twitterOptions);
                

            var fbOptions = new FacebookAuthenticationOptions {
                AppId = "924546580895867",
                AppSecret = "e1b0ce0f6260262e5776b15432aa5ae1",
                CallbackPath = new PathString("/account/signin-facebook"),
            };
            fbOptions.Scope.Add("email public_profile");
            app.UseFacebookAuthentication(fbOptions);


            var gOptions = new GoogleOAuth2AuthenticationOptions {
                ClientId = "1013789043506-8jtds0h8umno3sa11qdfkv66ej4qofpt.apps.googleusercontent.com",
                ClientSecret = "xA3GRBSaTMQKR6D7vrgniL_W",
                CallbackPath = new PathString("/account/signin-google")
            };
            gOptions.Scope.Add("email");
            app.UseGoogleAuthentication(gOptions);


            var linkedInOptions = new LinkedInAuthenticationOptions {
                ClientId = "775ktbbtdoo9c3",
                ClientSecret = "BuZGr6h4ZQtQrKCD",
                CallbackPath = new PathString("/account/signin-linkedin")
            };
            // linkedInOptions.Scope.Add("email"); // they have r_email by default
            app.UseLinkedInAuthentication(linkedInOptions);

            var yahooOptions = new YahooAuthenticationOptions {
                ConsumerKey = "dj0yJmk9MFRiaXpSbVVkOWt1JmQ9WVdrOVFVcDRZV1JHTkdrbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1mYQ--",
                ConsumerSecret = "6c2f0aa4e2457c633f0729ecbbfb6d76b89811d4",
                CallbackPath = new PathString("/account/signin-yahoo")
            };
            app.UseYahooAuthentication(yahooOptions);

        }
    }
}
