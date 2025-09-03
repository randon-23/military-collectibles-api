using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipment");
            builder.ToTable(t => t.HasCheckConstraint(
                "chk_EquipmentType",
                "EquipmentType IN ('Uniform', 'Armour', 'Inventory')")
            );
            builder.HasOne(e => e.StorageAreaDetails)
                .WithMany(loc => loc.Equipments)
                .HasForeignKey(e => e.StorageArea)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
