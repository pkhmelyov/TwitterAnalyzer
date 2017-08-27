using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TwitterAnalyzer.Data.Entities;

namespace TwitterAnalyzer.Data
{
    public class TwitterAnalyzerDbContext : IdentityDbContext<TwitterUser>
    {
        public TwitterAnalyzerDbContext() : base("TwitterAnalyzerConnection") { }

        public static TwitterAnalyzerDbContext Create()
        {
            return new TwitterAnalyzerDbContext();
        }

        public DbSet<ReportItem> ReportItems { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
