using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class InsigniaConfiguration : IEntityTypeConfiguration<Insignia>
    {
        public void Configure(EntityTypeBuilder<Insignia> builder)
        {
            builder.ToTable("Insignias");

            builder.HasOne(i => i.InsigniaSeries) // Navigation property for LiteratureSeries
                .WithMany(s => s.Insignias)
                .HasForeignKey(i => i.SeriesId)
                .IsRequired(false) // SeriesId is optional
                .OnDelete(DeleteBehavior.SetNull); // When the second to last insignia in a series is deleted, the SeriesId will be set to null instead of deleting the series; handled by API logic

            builder.HasOne(i => i.StorageAreaDetails) //navigation property references Location, and defines relationship, then ties back to the foreign key in Insignia
                .WithMany(loc => loc.Insignias)
                .HasForeignKey(i => i.StorageArea)
                .IsRequired(false) // Location is optional
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.InsigniaType)
                .WithMany(it => it.Insignias)
                .HasForeignKey(i => i.InsigniaTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Era)
                .WithMany(e => e.Insignias)
                .HasForeignKey(i => i.EraId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.Origin)
                .WithMany(o => o.Insignias)
                .HasForeignKey(i => i.OriginId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.Material)
                .WithMany(m => m.Insignias)
                .HasForeignKey(i => i.MaterialId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
