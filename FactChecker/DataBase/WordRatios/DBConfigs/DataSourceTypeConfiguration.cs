using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactChecker.DataBase.WordRatios.DBConfigs
{
    public class DataSourceTypeConfiguration : IEntityTypeConfiguration<DataSource>
    {
        public void Configure(EntityTypeBuilder<DataSource> ds)
        {
            ds.HasMany(p => p.Documents).WithOne(p => p.Publisher);
        }
    }
}
