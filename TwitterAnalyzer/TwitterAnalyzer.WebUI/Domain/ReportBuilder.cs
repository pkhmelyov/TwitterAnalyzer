using System;
using System.Linq;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Domain
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly ITweetsInfoProvider _tweetsProvider;

        public ReportBuilder(ITweetsInfoProvider tweetsProvider)
        {
            _tweetsProvider = tweetsProvider;
        }

        public void BuildReport(Report report)
        {
            TweetInfo[] tweets = _tweetsProvider.GetRecentTweets(report.UserName);
            var details = tweets.GroupBy(tweet => tweet.PostDate.Hour).Select(
                group => new ReportItem
                {
                    Hour = group.Key,
                    TweetsCount = group.Count(),
                    LikesCount = group.Sum(tweet => tweet.FavoritesCount),
                    LikesMedian = CalculateMedian(group.Select(tweet => tweet.FavoritesCount).ToArray())
                }).OrderBy(record => record.Hour).ToArray();
            var groupedReport = details.GroupBy(item => item.LikesMedian).OrderBy(group => group.Key);
            if (groupedReport.Count() > 1)
            {
                groupedReport.First().OrderBy(item => item.LikesCount).First().IsWorst = true;
                groupedReport.Last().OrderByDescending(item => item.LikesCount).First().IsBest = true;
            }

            report.UpdatedOn = DateTime.UtcNow;
            report.TotalTweetsCount = tweets.Length;
            report.TotalLikesCount = tweets.Sum(tweet => tweet.FavoritesCount);
            report.TotalLikesMedian = CalculateMedian(tweets.Select(tweet => tweet.FavoritesCount).ToArray());
            report.BestHour = details.FirstOrDefault(x => x.IsBest).Hour;
            report.ReportItems = details;
        }

        public Report BuildReport(string userName)
        {
            var result = new Report { UserName = userName };
            BuildReport(result);
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