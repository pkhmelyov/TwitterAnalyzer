using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.Data.Repositories
{
    public interface IReportRepository
    {
        Task<Report[]> GetRecentReportsAsync(string ownerId, int page, int pageSize);
        Task<Report> GetReportAsync(string userName);
        void AddReport(Report report);
        void DeleteItems(IEnumerable<ReportItem> items);
        Task SaveChangesAsync();
    }
}