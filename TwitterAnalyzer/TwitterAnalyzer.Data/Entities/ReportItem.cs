namespace TwitterAnalyzer.Data.Entities
{
    public class ReportItem
    {
        public int Hour { get; set; }
        public int TweetsCount { get; set; }
        public int LikesCount { get; set; }
        public double LikesMedian { get; set; }
        public bool IsBest { get; set; }
        public bool IsWorst { get; set; }
    }
}
