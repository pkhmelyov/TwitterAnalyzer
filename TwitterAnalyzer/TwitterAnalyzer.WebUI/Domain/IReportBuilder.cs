using LinqToTwitter;
using System.Collections.Generic;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface IReportBuilder
    {
        List<Status> GetUserStatuses(string userName);
    }
}