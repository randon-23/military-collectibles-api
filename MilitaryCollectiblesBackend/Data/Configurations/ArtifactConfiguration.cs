using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class ArtifactConfiguration : IEntityTypeConfiguration<Artifact>
    {
        public void Configure(EntityTypeBuilder<Artifact> builder)
        {
            builder.ToTable("Artifacts");

            builder.HasOne(a => a.ArtifactSeries) // Navigation property for LiteratureSeries
                .WithMany(s => s.Artifacts)
                .HasForeignKey(a => a.SeriesId)
                .IsRequired(false) // SeriesId is optional
                .OnDelete(DeleteBehavior.SetNull); // When the second to last insignia in a series is deleted, the SeriesId will be set to null instead of deleting the series; handled by API logic

            builder.HasOne(a => a.StorageAreaDetails) //navigation property references Location, and defines relationship, then ties back to the foreign key in Insignia
                .WithMany(loc => loc.Artifacts)
                .HasForeignKey(a => a.StorageArea)
                .IsRequired(false) // Location is optional
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.ArtifactType)
                .WithMany(at => at.Artifacts)
                .HasForeignKey(a => a.ArtifactTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Era)
                .WithMany(e => e.Artifacts)
                .HasForeignKey(a => a.EraId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Origin)
                .WithMany(o => o.Artifacts)
                .HasForeignKey(a => a.OriginId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
