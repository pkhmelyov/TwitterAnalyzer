using System.Web.Mvc;
using TwitterAnalyzer.WebUI.Domain;
using TwitterAnalyzer.WebUI.Models.Home;

namespace TwitterAnalyzer.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReportBuilder _reportBuilder;

        public HomeController(IReportBuilder reportBuilder)
        {
            _reportBuilder = reportBuilder;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(string username)
        {
            
            var model = new About {Username = username, Report = _reportBuilder.BuildReport(username)};
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}