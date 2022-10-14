using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactChecker.DataBase.WordRatios.DBConfigs
{
    public class WordRatioTypeConfiguration : IEntityTypeConfiguration<WordRatio>
    {
        public void Configure(EntityTypeBuilder<WordRatio> we)
        {
            we.HasOne(p => p.Document).WithMany(p => p.WordRatios);
        }
    }
}
