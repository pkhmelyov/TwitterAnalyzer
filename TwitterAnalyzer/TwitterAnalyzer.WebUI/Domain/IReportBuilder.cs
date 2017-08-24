using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface IReportBuilder
    {
        ReportItem[] BuildReport(string userName);
    }
}