using System.Web;
using Microsoft.AspNet.Identity;
using TwitterAnalyzer.Data.Entities;
using TwitterAnalyzer.Data.Repositories;

namespace TwitterAnalyzer.WebUI.Domain
{
    public class ReportManager : IReportManager
    {
        private readonly int _pageSize = 10;
        private readonly IReportRepository _reportRepository;
        private readonly IReportBuilder _reportBuilder;
        private readonly string _currentUserId;

        public ReportManager(IReportRepository reportrepository, IReportBuilder reportBuilder, HttpContextBase httpContext)
        {
            _reportRepository = reportrepository;
            _reportBuilder = reportBuilder;
            _currentUserId = httpContext.User.Identity.GetUserId();
        }

        public Report[] GetRecentReports(int page)
        {
            return _reportRepository.GetRecentReports(ownerId: null, page: page <= 0 ? 1 : page, pageSize: _pageSize);
        }

        public Report[] GetRecentReportsForCurrentUser()
        {
            return _reportRepository.GetRecentReports(ownerId: _currentUserId, page: 1, pageSize: _pageSize);
        }

        public Report GetReport(string userName)
        {
            userName = userName.Trim().ToLower();
            var report = _reportRepository.GetReport(userName);
            if (report == null)
            {
                report = _reportBuilder.BuildReport(userName);
                report.UserId = _currentUserId;
                _reportRepository.AddReport(report);
                _reportRepository.SaveChanges();
            }
            return report;
        }
    }
}