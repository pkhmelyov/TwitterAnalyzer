using System;
using System.Linq;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.Data.Repositories
{
    internal class ReportRepository : IReportRepository
    {
        private readonly TwitterAnalyzerDbContext _context;

        public ReportRepository(TwitterAnalyzerDbContext context)
        {
            _context = context;
        }

        public Report[] GetRecentReports(string ownerId, int page, int pageSize)
        {
            Func<Report, bool> predicate;
            if (string.IsNullOrWhiteSpace(ownerId)) predicate = report => true;
            else predicate = report => report.UserId == ownerId;
            return
                _context.Reports.Where(predicate)
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize)
                    .ToArray();
        }

        public Report GetReport(string userName)
        {
            return _context.Reports.FirstOrDefault(report => report.UserName == userName);
        }

        public void AddReport(Report report)
        {
            _context.Reports.Add(report);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}