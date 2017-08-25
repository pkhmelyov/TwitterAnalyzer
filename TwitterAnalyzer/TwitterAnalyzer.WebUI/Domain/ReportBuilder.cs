using System;
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

        public Report BuildReport(string userName)
        {
            List<Status> tweets = GetUserStatuses(userName);
            var details = tweets.GroupBy(tweet => tweet.CreatedAt.Hour).Select(
                group => new ReportItem
                {
                    Hour = group.Key,
                    TweetsCount = group.Count(),
                    LikesCount = group.Sum(tweet => tweet.FavoriteCount ?? 0),
                    LikesMedian = CalculateMedian(group.Select(tweet => tweet.FavoriteCount ?? 0).ToArray())
                }).OrderBy(record => record.Hour).ToArray();
            var groupedReport = details.GroupBy(item => item.LikesMedian).OrderBy(group => group.Key);
            if (groupedReport.Count() > 1)
            {
                groupedReport.First().OrderBy(item => item.LikesCount).First().IsWorst = true;
                groupedReport.Last().OrderByDescending(item => item.LikesCount).First().IsBest = true;
            }
            var result = new Report
            {
                UserName = userName,
                UpdatedOn = DateTime.UtcNow,
                TotalTweetsCount = tweets.Count,
                TotalLikesCount = tweets.Sum(tweet => tweet.FavoriteCount ?? 0),
                TotalLikesMedian = CalculateMedian(tweets.Select(tweet => tweet.FavoriteCount ?? 0).ToArray()),
                BestHour = details.FirstOrDefault(x=>x.IsBest).Hour,
                ReportItems = details
            };
            return result;
        }

        private double CalculateMedian(int[] numbers)
        {
            if (numbers.Length%2 == 0)
                return numbers.OrderBy(x => x).Skip(numbers.Length/2 - 1).Take(2).Sum()/2;
            else
                return numbers.OrderBy(x => x).Skip(numbers.Length/2).First();
        }
    }
}