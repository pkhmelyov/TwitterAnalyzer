using System.Web;
using Microsoft.AspNet.Identity;
using TwitterAnalyzer.Data.Entities;
using TwitterAnalyzer.Data.Repositories;
using System.Threading.Tasks;

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

        public Task<Report[]> GetRecentReportsAsync(int page)
        {
            return _reportRepository.GetRecentReportsAsync(ownerId: null, page: page <= 0 ? 1 : page, pageSize: _pageSize);
        }

        public Task<Report[]> GetRecentReportsForCurrentUserAsync()
        {
            return _reportRepository.GetRecentReportsAsync(ownerId: _currentUserId, page: 1, pageSize: _pageSize);
        }

        public async Task<Report> GetReportAsync(string userName)
        {
            userName = userName.Trim().ToLower();
            var report = await _reportRepository.GetReportAsync(userName);
            if (report == null)
            {
                report = await _reportBuilder.BuildReportAsync(userName);
                if(report != null)
                {
                    report.UserId = _currentUserId;
                    _reportRepository.AddReport(report);
                    await _reportRepository.SaveChangesAsync();
                }
            }
            return report;
        }

        public async Task RegenerateReportAsync(string userName)
        {
            userName = userName.Trim().ToLower();
            var report = await _reportRepository.GetReportAsync(userName);
            if (report != null)
            {
                await ClearReportAsync(report);
                if(await _reportBuilder.BuildReportAsync(report))
                    await _reportRepository.SaveChangesAsync();
            }
        }

        public async Task ClearReportAsync(Report report)
        {
            _reportRepository.DeleteItems(report.ReportItems);
            report.ReportItems.Clear();
            await _reportRepository.SaveChangesAsync();
        }
    }
}