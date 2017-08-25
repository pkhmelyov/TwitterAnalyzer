using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface IReportManager
    {
        Report[] GetRecentReports(int page);
        Report[] GetRecentReportsForCurrentUser();
        Report GetReport(string userName);
    }
}