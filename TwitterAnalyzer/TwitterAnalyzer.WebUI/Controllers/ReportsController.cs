using System.Threading.Tasks;
using System.Web.Mvc;
using LinqToTwitter;
using TwitterAnalyzer.WebUI.Infrastructure;

namespace TwitterAnalyzer.WebUI.Controllers
{
    public class ReportsController : Controller
    {
        private FixedSessionStateCredentialStore _credentialStore;
        private FixedSessionStateCredentialStore CredentialStore
        {
            get { return _credentialStore ?? (_credentialStore = new FixedSessionStateCredentialStore()); }
        }

        private MvcAuthorizer _authorizer;
        private MvcAuthorizer Authorizer
        {
            get { return _authorizer ?? (_authorizer = new MvcAuthorizer {CredentialStore = CredentialStore}); }
        }

        private TwitterContext _twitterContext;
        private TwitterContext TwitterContext
        {
            get { return _twitterContext ?? (_twitterContext = new TwitterContext(Authorizer)); }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string username)
        {
            TwitterContext
            return View();
        }
    }
}