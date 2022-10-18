using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactChecker.DataBase.WordRatios.DBConfigs
{
    public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> c)
        {
            c.HasMany(p => p.Documents).WithOne(p => p.Category);
        }
    }
}
