using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class StorageAreaConfiguration : IEntityTypeConfiguration<StorageArea>
    {
        public void Configure(EntityTypeBuilder<StorageArea> builder)
        {
            builder.ToTable("StorageArea");

            builder.HasMany(st => st.Literatures)
                .WithOne(l => l.StorageAreaDetails)
                .HasForeignKey(l => l.StorageArea)
                .IsRequired(false);
            //On Delete not needed here as the delete behavior is handled in LiteratureConfiguration

            builder.HasMany(st => st.Insignias)
                .WithOne(i => i.StorageAreaDetails)
                .HasForeignKey(i => i.StorageArea)
                .IsRequired(false);
            //On Delete not needed here as the delete behavior is handled in InsigniaConfiguration

            builder.HasMany(st => st.Artifacts)
                .WithOne(a => a.StorageAreaDetails)
                .HasForeignKey(a => a.StorageArea)
                .IsRequired(false);
            //On Delete not needed here as the delete behavior is handled in ArtifactConfiguration

            builder.HasMany(st => st.Equipments)
                .WithOne(e => e.StorageAreaDetails)
                .HasForeignKey(e => e.StorageArea)
                .IsRequired(false);
            //On Delete not needed here as the delete behavior is handled in EquipmentConfiguration

            builder.HasMany(st => st.MechanicalEquipments)
                .WithOne(me => me.StorageAreaDetails)
                .HasForeignKey(me => me.StorageArea)
                .IsRequired(false);
            //On Delete not needed here as the delete behavior is handled in MechanicalEquipmentConfiguration
        }
    }
}
