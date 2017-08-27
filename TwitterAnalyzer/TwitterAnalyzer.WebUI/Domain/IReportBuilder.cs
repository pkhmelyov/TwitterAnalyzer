using System.Threading.Tasks;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface IReportBuilder
    {
        Task<Report> BuildReportAsync(string userName);
        Task<bool> BuildReportAsync(Report report);
    }
}