using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class ArtifactSeriesConfiguration : IEntityTypeConfiguration<ArtifactSeries>
    {
        public void Configure(EntityTypeBuilder<ArtifactSeries> builder)
        {
            builder.ToTable("ArtifactSeries");
            builder.HasMany(ars => ars.Artifacts)
                .WithOne(a => a.ArtifactSeries)
                .HasForeignKey(a => a.SeriesId)
                .IsRequired(false);
        }
    }
}
