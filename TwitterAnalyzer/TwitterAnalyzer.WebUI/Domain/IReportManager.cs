using System.Threading.Tasks;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface IReportManager
    {
        Task<Report[]> GetRecentReportsAsync(int page);
        Task<Report[]> GetRecentReportsForCurrentUserAsync();
        Task<Report> GetReportAsync(string userName);
        Task RegenerateReportAsync(string userName);
    }
}