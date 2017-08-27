using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<Report[]> GetRecentReportsAsync(string ownerId, int page, int pageSize)
        {
            return
                _context.Reports.Where(report => ownerId == null || report.UserId == ownerId)
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();
        }

        public Task<Report> GetReportAsync(string userName)
        {
            return _context.Reports
                .Include(report => report.ReportItems)
                .FirstOrDefaultAsync(report => report.UserName == userName);
        }

        public void AddReport(Report report)
        {
            _context.Reports.Add(report);
        }

        public void DeleteItems(IEnumerable<ReportItem> items)
        {
            _context.ReportItems.RemoveRange(items);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}