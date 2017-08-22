using System.Threading.Tasks;
using LinqToTwitter;

namespace TwitterAnalyzer.WebUI.Infrastructure
{
    public class FixedSessionStateCredentialStore : SessionStateCredentialStore
    {
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