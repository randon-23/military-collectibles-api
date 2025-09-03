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
            builder.ToTable(t => t.HasCheckConstraint(
                "chk_InsigniaType",
                "InsigniaType IN  ('Badge', 'Regimental Badge', 'Lapel Badge', 'Ribbon')")
            );

            builder.HasOne(i => i.InsigniaSeries) // Navigation property for LiteratureSeries
                .WithMany(s => s.Insignias)
                .HasForeignKey(i => i.SeriesId)
                .IsRequired(false) // SeriesId is optional
                .OnDelete(DeleteBehavior.SetNull); // When the second to last insignia in a series is deleted, the SeriesId will be set to null instead of deleting the series; handled by API logic

            builder.HasOne(i => i.StorageAreaDetails) //navigation property references Location, and defines relationship, then ties back to the foreign key in Insignia
                .WithMany(loc => loc.Insignias)
                .HasForeignKey(i => i.StorageArea)
                .IsRequired(false) // Location is optional
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
