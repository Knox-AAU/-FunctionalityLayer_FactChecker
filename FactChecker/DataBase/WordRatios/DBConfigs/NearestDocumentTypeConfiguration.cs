using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactChecker.DataBase.WordRatios.DBConfigs
{
    public class NearestDocumentTypeConfiguration : IEntityTypeConfiguration<NearestDocument>
    {
        public void Configure(EntityTypeBuilder<NearestDocument> nd)
        {
            nd.HasOne(p => p.Main).WithMany(p => p.SimilarDocuments);
        }
    }
}
