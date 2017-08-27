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

        public ActionResult Index()
        {
            Report[] reports;
            if (Request.IsAuthenticated)
                reports = _reportManager.GetRecentReportsForCurrentUser();
            else
                reports = _reportManager.GetRecentReports(1);
            return View(reports);
        }

        public ActionResult About(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Trim() == "@") userName = User.Identity.Name;
            var model = _reportManager.GetReport(userName);
            return View(model);
        }

        [HttpPost]
        [ActionName("About")]
        [ValidateAntiForgeryToken]
        public ActionResult AboutPost(string userName)
        {
            _reportManager.RegenerateReport(userName);
            return RedirectToAction("About", "Home", new { userName });
        }
    }
}