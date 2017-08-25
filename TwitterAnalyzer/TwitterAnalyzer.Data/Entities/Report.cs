using System;
using System.Collections.Generic;

namespace TwitterAnalyzer.Data.Entities
{
    public class Report
    {
        public Report()
        {
            ReportItems = new List<ReportItem>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserName { get; set; }
        public int TotalTweetsCount { get; set; }
        public int TotalLikesCount { get; set; }
        public double TotalLikesMedian { get; set; }
        public int? BestHour { get; set; }
        public virtual ICollection<ReportItem> ReportItems { get; set; }
    }
}
