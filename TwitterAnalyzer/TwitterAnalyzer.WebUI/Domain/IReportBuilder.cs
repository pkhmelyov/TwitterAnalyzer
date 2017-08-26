using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface IReportBuilder
    {
        Report BuildReport(string userName);
        void BuildReport(Report report);
    }
}