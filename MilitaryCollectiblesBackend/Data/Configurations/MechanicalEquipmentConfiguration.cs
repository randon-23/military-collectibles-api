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
            //builder.ToTable(t => t.HasCheckConstraint(
            //    "chk_MechanicalEquipmentType",
            //    "MechanicalEquipmentType IN ('Ordinance', 'Weapon', 'Projectile', 'Vehicular')")
            //);
            builder.HasOne(m => m.StorageAreaDetails)
                .WithMany(loc => loc.MechanicalEquipments)
                .HasForeignKey(m => m.StorageArea)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.MechanicalEquipmentType)
                .WithMany(met => met.MechanicalEquipments)
                .HasForeignKey(m => m.MechanicalEquipmentTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Era)
                .WithMany(er => er.MechanicalEquipments)
                .HasForeignKey(m => m.EraId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Origin)
                .WithMany(o => o.MechanicalEquipments)
                .HasForeignKey(m => m.OriginId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Material)
                .WithMany(mat => mat.MechanicalEquipments)
                .HasForeignKey(m => m.MaterialId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.CaliberSpec)
                .WithMany(c => c.MechanicalEquipments)
                .HasForeignKey(m => m.CaliberSpecId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Manufacturer)
                .WithMany(ma => ma.MechanicalEquipments)
                .HasForeignKey(m => m.ManufacturerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
