using System.Collections.Generic;
using LinqToTwitter;
using System.Linq;

namespace TwitterAnalyzer.WebUI.Domain
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly TwitterContext _twitterContext;

        public ReportBuilder(TwitterContext twitterContext)
        {
            _twitterContext = twitterContext;
        }

        public List<Status> GetUserStatuses(string userName)
        {
            ulong maxID;
            int count = 200;
            var statusList = new List<Status>();
            var userStatusResponse =
                _twitterContext.Status.Where(x => x.Type == StatusType.User && x.ScreenName == userName && x.Count == count)
                    .ToList();
            if (userStatusResponse.Any())
            {
                count = 1000 - statusList.Count;
                if (count > 200) count = 200;
                statusList.AddRange(userStatusResponse);
                maxID = userStatusResponse.Min(status => status.StatusID) - 1;
                while (userStatusResponse.Count != 0 && statusList.Count < 1000)
                {

                    userStatusResponse =
                        _twitterContext.Status.Where(
                            x =>
                                x.Type == StatusType.User && x.ScreenName == userName && x.Count == count &&
                                x.MaxID == maxID).ToList();
                    if (userStatusResponse.Any())
                    {
                        maxID = userStatusResponse.Min(status => status.StatusID) - 1;
                        statusList.AddRange(userStatusResponse);
                    }
                }
            }
            return statusList;
        }
    }
}