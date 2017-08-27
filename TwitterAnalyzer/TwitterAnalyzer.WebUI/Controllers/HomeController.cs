using System.Threading.Tasks;
using System.Web.Mvc;
using TwitterAnalyzer.Data.Entities;
using TwitterAnalyzer.WebUI.Domain;

namespace TwitterAnalyzer.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReportManager _reportManager;

        public HomeController(IReportManager reportManager)
        {
            _reportManager = reportManager;
        }

        public async Task<ActionResult> Index()
        {
            Report[] reports;
            if (Request.IsAuthenticated)
                reports = await _reportManager.GetRecentReportsForCurrentUserAsync();
            else
                reports = await _reportManager.GetRecentReportsAsync(1);
            return View(reports);
        }

        public async Task<ActionResult> About(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Trim() == "@") userName = User.Identity.Name;
            var model = await _reportManager.GetReportAsync(userName);
            return View(model);
        }

        [HttpPost]
        [ActionName("About")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AboutPost(string userName)
        {
            await _reportManager.RegenerateReportAsync(userName);
            return RedirectToAction("About", "Home", new { userName });
        }
    }
}