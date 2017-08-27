using System.Threading.Tasks;

namespace TwitterAnalyzer.WebUI.Domain
{
    public interface ITweetsInfoProvider
    {
        Task<TweetInfo[]> GetRecentTweetsAsync(string userName);
    }
}