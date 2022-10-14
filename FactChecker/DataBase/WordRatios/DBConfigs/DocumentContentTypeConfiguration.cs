using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactChecker.DataBase.WordRatios.DBConfigs
{
    public class DocumentContentTypeConfiguration : IEntityTypeConfiguration<DocumentContent>
    {
        public void Configure(EntityTypeBuilder<DocumentContent> dc)
        {
            dc.HasOne(p => p.Document).WithMany(p => p.DocumentContents);
        }
    }
}
