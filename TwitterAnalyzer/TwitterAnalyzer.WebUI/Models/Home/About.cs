using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.WebUI.Models.Home
{
    public class About
    {
        public string Username { get; set; }
        public ReportItem[] Report { get; set; }
    }
}