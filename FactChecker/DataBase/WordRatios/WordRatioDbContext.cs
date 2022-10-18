using Microsoft.EntityFrameworkCore;

namespace FactChecker.DataBase.WordRatios
{
    public class WordRatioDbContext : DbContext
    {
        public WordRatioDbContext(DbContextOptions<WordRatioDbContext> opt) : base(opt) { }
        public WordRatioDbContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfigurationsFromAssembly(typeof(WordRatioDbContext).Assembly);
            base.OnModelCreating(model);
        }

        public DbSet<WordRatio> WordRatios { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<NearestDocument> NearestDocuments { get; set; }
        public DbSet<DocumentContent> DocumentContents { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
