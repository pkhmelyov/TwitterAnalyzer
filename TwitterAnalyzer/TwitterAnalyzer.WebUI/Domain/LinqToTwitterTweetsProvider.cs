using System;
using LinqToTwitter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterAnalyzer.WebUI.Domain
{
    public class LinqToTwitterTweetsProvider : ITweetsInfoProvider
    {
        private readonly TwitterContext _twitterContext;
        private readonly Func<Status, TweetInfo> _statusToTweetInfo =
            status => new TweetInfo {PostDate = status.CreatedAt, FavoritesCount = status.FavoriteCount ?? 0};
        private readonly int _maxTweetsCount = 1000;

        public LinqToTwitterTweetsProvider(TwitterContext twitterContext)
        {
            _twitterContext = twitterContext;
        }

        public async Task<TweetInfo[]> GetRecentTweetsAsync(string userName)
        {
            ulong maxID;
            int count = 200;
            var statusList = new List<TweetInfo>();
            try
            {
                var userStatusResponse = await
                _twitterContext.Status.Where(x => x.Type == StatusType.User && x.ScreenName == userName && x.Count == count)
                    .ToListAsync();
                if (userStatusResponse.Any())
                {
                    statusList.AddRange(userStatusResponse.Select(_statusToTweetInfo));
                    maxID = userStatusResponse.Min(status => status.StatusID) - 1;
                    while (userStatusResponse.Count != 0 && statusList.Count < _maxTweetsCount)
                    {
                        count = _maxTweetsCount - statusList.Count;
                        if (count > 200) count = 200;
                        userStatusResponse = await
                            _twitterContext.Status.Where(
                                x =>
                                    x.Type == StatusType.User && x.ScreenName == userName && x.Count == count &&
                                    x.MaxID == maxID).ToListAsync();
                        if (userStatusResponse.Any())
                        {
                            maxID = userStatusResponse.Min(status => status.StatusID) - 1;
                            statusList.AddRange(userStatusResponse.Select(_statusToTweetInfo));
                        }
                    }
                }
                return statusList.ToArray();
            }
            catch(AggregateException ex)
            {
                if (ex.InnerExceptions.Any(x => x is TwitterQueryException))
                    return null;
                else throw;
            }
        }
    }
}