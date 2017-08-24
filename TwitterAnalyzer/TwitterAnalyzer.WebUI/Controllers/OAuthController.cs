using System.Web.Mvc;
using LinqToTwitter;
using System.Threading.Tasks;
using System;
using TwitterAnalyzer.Data.Entities;
using Microsoft.AspNet.Identity;

namespace TwitterAnalyzer.WebUI.Controllers
{
    public class OAuthController : Controller
    {
        private readonly MvcSignInAuthorizer _mvcAuthorizer;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;

        public OAuthController(MvcSignInAuthorizer mvcAuthorizer, ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _mvcAuthorizer = mvcAuthorizer;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ActionResult> Begin()
        {
            string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete");
            return await _mvcAuthorizer.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
        }

        public async Task<ActionResult> Complete()
        {
            await _mvcAuthorizer.CompleteAuthorizeAsync(Request.Url);
            var credentials = _mvcAuthorizer.CredentialStore;

            var user = await _userManager.FindByNameAsync(credentials.ScreenName);
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
                identityResult = await _userManager.CreateAsync(user);
            }
            if(identityResult == null || identityResult.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Home");
        }
    }
}