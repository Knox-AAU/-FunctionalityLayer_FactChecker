using FactChecker.WordcountDB;
using Microsoft.EntityFrameworkCore;

namespace FactChecker.EF
{
    public class KnoxFactCheckingTestDbContext : DbContext
    {
        public KnoxFactCheckingTestDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<ArticleItem>().Property(b => b.id).UseIdentityAlwaysColumn();
        }
        public DbSet<ArticleItem> article { get; set; }
        public DbSet<WordCountItem> wordcount { get; set; }
        public DbSet<StopWordItem> stopwords { get; set; }
        public DbSet<TripleItem> triples { get; set; }
    }

    
}
