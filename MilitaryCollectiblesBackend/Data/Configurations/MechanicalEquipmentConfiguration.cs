using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    public class MechanicalEquipmentConfiguration : IEntityTypeConfiguration<MechanicalEquipment>
    {
        public void Configure(EntityTypeBuilder<MechanicalEquipment> builder)
        {
            builder.ToTable("MechanicalEquipment");
            builder.ToTable(t => t.HasCheckConstraint(
                "chk_MechanicalEquipmentType",
                "MechanicalEquipmentType IN ('Ordinance', 'Weapon', 'Projectile', 'Vehicular')")
            );
            builder.HasOne(m => m.StorageAreaDetails)
                .WithMany(loc => loc.MechanicalEquipments)
                .HasForeignKey(m => m.StorageArea)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
