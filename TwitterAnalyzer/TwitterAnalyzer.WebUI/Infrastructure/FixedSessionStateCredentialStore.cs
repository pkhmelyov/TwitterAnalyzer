using System.Threading.Tasks;
using System.Web;
using LinqToTwitter;
using System.Configuration;

namespace TwitterAnalyzer.WebUI.Infrastructure
{
    public class FixedSessionStateCredentialStore : SessionStateCredentialStore
    {
        public FixedSessionStateCredentialStore(HttpContextBase httpContext, ApplicationUserManager userManager)
        {
            if (!HasAllCredentials() && httpContext.Request.IsAuthenticated)
            {
                var user = userManager.FindByNameAsync(httpContext.User.Identity.Name).Result;
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"];
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                OAuthToken = user.OAuthToken;
                OAuthTokenSecret = user.OAuthTokenSecret;
            }
        }

        public override Task ClearAsync()
        {
            ConsumerKey = null;
            ConsumerSecret = null;
            OAuthToken = null;
            OAuthTokenSecret = null;
            return base.ClearAsync();
        }
    }
}