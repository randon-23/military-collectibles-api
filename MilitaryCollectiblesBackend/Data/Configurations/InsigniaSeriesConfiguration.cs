using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class InsigniaSeriesConfiguration : IEntityTypeConfiguration<InsigniaSeries>
    {
        public void Configure(EntityTypeBuilder<InsigniaSeries> builder)
        {
            builder.ToTable("InsigniaSeries");
            builder.HasMany(ins => ins.Insignias)
                .WithOne(i => i.InsigniaSeries)
                .HasForeignKey(i => i.SeriesId)
                .IsRequired(false);
        }
    }
}
