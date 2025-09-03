using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class LiteratureSeriesConfiguration : IEntityTypeConfiguration<LiteratureSeries>
    {
        public void Configure(EntityTypeBuilder<LiteratureSeries> builder)
        {
            builder.ToTable("LiteratureSeries");

            builder.HasMany(ls => ls.Literatures)
                .WithOne(l => l.LiteratureSeries)
                .HasForeignKey(l => l.SeriesId)
                .IsRequired(false);
                //On Delete not needed here as the delete behavior is handled in LiteratureConfiguration
        }
    }
}
