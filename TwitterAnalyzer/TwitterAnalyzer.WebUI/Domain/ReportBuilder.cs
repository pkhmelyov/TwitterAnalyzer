using System.Collections.Generic;
using LinqToTwitter;
using System.Linq;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly TwitterContext _twitterContext;

        public ReportBuilder(TwitterContext twitterContext)
        {
            _twitterContext = twitterContext;
        }

        private List<Status> GetUserStatuses(string userName)
        {
            ulong maxID;
            int count = 200;
            var statusList = new List<Status>();
            var userStatusResponse =
                _twitterContext.Status.Where(x => x.Type == StatusType.User && x.ScreenName == userName && x.Count == count)
                    .ToList();
            if (userStatusResponse.Any())
            {
                statusList.AddRange(userStatusResponse);
                maxID = userStatusResponse.Min(status => status.StatusID) - 1;
                while (userStatusResponse.Count != 0 && statusList.Count < 1000)
                {
                    count = 1000 - statusList.Count;
                    if (count > 200) count = 200;
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

        public ReportItem[] BuildReport(string userName)
        {
            List<Status> tweets = GetUserStatuses(userName);
            var report = tweets.GroupBy(tweet => tweet.CreatedAt.Hour).Select(
                group => new ReportItem
                {
                    Hour = group.Key,
                    TweetsCount = group.Count(),
                    LikesCount = group.Sum(tweet => tweet.FavoriteCount ?? 0),
                    LikesMedian = 
                        group.Count()%2 == 1
                            ? group.OrderBy(tweet => tweet.FavoriteCount)
                                .Skip(group.Count()/2)
                                .Take(1)
                                .Single()
                                .FavoriteCount ?? 0
                            : group.OrderBy(tweet => tweet.FavoriteCount)
                                .Skip(group.Count()/2 - 1)
                                .Take(2)
                                .Average(tweet => tweet.FavoriteCount ?? 0)
                }).OrderBy(record => record.Hour).ToArray();
            var groupedReport = report.GroupBy(item => item.LikesMedian).OrderBy(group => group.Key);
            if (groupedReport.Count() > 1)
            {
                groupedReport.First().OrderBy(item => item.LikesCount).First().IsWorst = true;
                groupedReport.Last().OrderByDescending(item => item.LikesCount).First().IsBest = true;
            }
            return report;
        }
    }
}