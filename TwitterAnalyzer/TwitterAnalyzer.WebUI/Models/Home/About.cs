using System.Collections.Generic;
using LinqToTwitter;

namespace TwitterAnalyzer.WebUI.Models.Home
{
    public class About
    {
        public string Username { get; set; }
        public List<Status> Tweets { get; set; }
    }
}