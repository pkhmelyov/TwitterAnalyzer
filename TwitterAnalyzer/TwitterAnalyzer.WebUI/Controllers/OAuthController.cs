using System.Web.Mvc;
using LinqToTwitter;
using System.Threading.Tasks;
using System;
using TwitterAnalyzer.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace TwitterAnalyzer.WebUI.Controllers
{
    public class OAuthController : Controller
    {
        private readonly MvcSignInAuthorizer _mvcAuthorizer;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public OAuthController(MvcSignInAuthorizer mvcAuthorizer, ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _mvcAuthorizer = mvcAuthorizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        public async Task<ActionResult> Begin()
        {
            string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete");
            return await _mvcAuthorizer.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
        }

        public async Task<ActionResult> Complete(string denied)
        {
            if (!string.IsNullOrWhiteSpace(denied)) return RedirectToAction("Index", "Home");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            _mvcAuthorizer.CredentialStore.ClearAsync().Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}