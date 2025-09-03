using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class LiteratureConfiguration : IEntityTypeConfiguration<Literature>
    {
        public void Configure(EntityTypeBuilder<Literature> builder)
        {
            builder.ToTable("Literatures");

            builder.ToTable(t => t.HasCheckConstraint(
                "chk_LiteratureType",
                "LiteratureType IN ('Book', 'Magazine')")
            ); // need the ToTable lambda as they are applied at the db level, not the entity model level

            builder.ToTable(t => t.HasCheckConstraint(
                "chk_BindingType",
                "BindingType IN ('Paperback', 'Hardback')")
            );

            // Does not need the ToTable lambda as it is applied at the entity model level, models entity relationships
            builder.HasOne(l => l.LiteratureSeries) // Navigation property for LiteratureSeries
                .WithMany(s => s.Literatures)
                .HasForeignKey(l => l.SeriesId)
                .IsRequired(false) // SeriesId is optional
                .OnDelete(DeleteBehavior.SetNull); // When the second to last literature in a series is deleted, the SeriesId will be set to null instead of deleting the series; handled by API logic

            builder.HasOne(l => l.StorageAreaDetails) //navigation property references Location, and defines relationship, then ties back to the foreign key in Literature
                .WithMany(loc => loc.Literatures)
                .HasForeignKey(l => l.StorageArea)
                .IsRequired(false) // Location is optional
                .OnDelete(DeleteBehavior.Cascade);

            // unique index on ISBN, allowing nulls (which does not mean allowing duplicates)
            builder.HasIndex(l => l.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");
        }
    }
}
