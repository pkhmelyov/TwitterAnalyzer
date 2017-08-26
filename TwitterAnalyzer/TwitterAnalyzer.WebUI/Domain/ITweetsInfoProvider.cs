namespace TwitterAnalyzer.WebUI.Domain
{
    public interface ITweetsInfoProvider
    {
        TweetInfo[] GetRecentTweets(string userName);
    }
}