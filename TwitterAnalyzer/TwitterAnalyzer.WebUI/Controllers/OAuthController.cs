using System.Configuration;
using System.Web.Mvc;
using LinqToTwitter;
using System.Threading.Tasks;
using System;
using TwitterAnalyzer.Data.Entities;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TwitterAnalyzer.WebUI.Infrastructure;

namespace TwitterAnalyzer.WebUI.Controllers
{
    public class OAuthController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private MvcSignInAuthorizer _mvcAuthorizer;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public MvcSignInAuthorizer Authorizer
        {
            get
            {
                return _mvcAuthorizer ?? (_mvcAuthorizer = new MvcSignInAuthorizer
                {
                    CredentialStore = new FixedSessionStateCredentialStore
                    {
                        ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                        ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"]
                    }
                });
            }
        }

        public async Task<ActionResult> BeginAsync()
        {
            string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete");
            return await Authorizer.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
        }

        public async Task<ActionResult> CompleteAsync()
        {
            await Authorizer.CompleteAuthorizeAsync(Request.Url);
            var credentials = Authorizer.CredentialStore;

            var user = await UserManager.FindByNameAsync(credentials.ScreenName);
            IdentityResult identityResult = null;
            if (user == null)
            {
                user = new TwitterUser
                {
                    UserName = credentials.ScreenName,
                    OAuthToken = credentials.OAuthToken,
                    OAuthTokenSecret = credentials.OAuthTokenSecret,
                    TwitterId = credentials.UserID
                };
                identityResult = await UserManager.CreateAsync(user);
            }
            if(identityResult == null || identityResult.Succeeded)
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Home");
        }
    }
}