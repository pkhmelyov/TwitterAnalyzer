using System.Collections.Generic;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.Data.Repositories
{
    public interface IReportRepository
    {
        Report[] GetRecentReports(string ownerId, int page, int pageSize);
        Report GetReport(string userName);
        void AddReport(Report report);
        void DeleteItems(IEnumerable<ReportItem> items);
        void SaveChanges();
    }
}