using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data.Configurations
{
    //Commented out OnDelete behaviors to prevent accidental cascading deletions. Deletions handled in enttity configurations.
    public class OriginConfiguration : IEntityTypeConfiguration<Origin>
    {
        public void Configure(EntityTypeBuilder<Origin> builder)
        {
            builder.ToTable("Origins");
            builder.HasKey(o => o.OriginId);
            builder.Property(o => o.OriginName).IsRequired().HasMaxLength(100);

            builder.HasMany(o => o.Artifacts)
                .WithOne(a => a.Origin)
                .HasForeignKey(a => a.OriginId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.Equipments)
                .WithOne(e => e.Origin)
                .HasForeignKey(e => e.OriginId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.Insignias)
                .WithOne(i => i.Origin)
                .HasForeignKey(i => i.OriginId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.MechanicalEquipments)
                .WithOne(me => me.Origin)
                .HasForeignKey(me => me.OriginId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EraConfiguration : IEntityTypeConfiguration<Era>
    {
        public void Configure(EntityTypeBuilder<Era> builder)
        {
            builder.ToTable("Eras");
            builder.HasKey(e => e.EraId);
            builder.Property(e => e.EraName).IsRequired().HasMaxLength(100);

            builder.HasMany(e => e.Artifacts)
                .WithOne(a => a.Era)
                .HasForeignKey(a => a.EraId)
                .IsRequired(false);
                ////.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Equipments)
                .WithOne(eq => eq.Era)
                .HasForeignKey(eq => eq.EraId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Insignias)
                .WithOne(i => i.Era)
                .HasForeignKey(i => i.EraId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.MechanicalEquipments)
                .WithOne(me => me.Era)
                .HasForeignKey(me => me.EraId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Materials");
            builder.HasKey(m => m.MaterialId);
            builder.Property(m => m.MaterialName).IsRequired().HasMaxLength(100);

            builder.HasMany(m => m.Equipments)
                .WithOne(e => e.Material)
                .HasForeignKey(e => e.MaterialId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.Insignias)
                .WithOne(i => i.Material)
                .HasForeignKey(i => i.MaterialId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.MechanicalEquipments)
                .WithOne(me => me.Material)
                .HasForeignKey(me => me.MaterialId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");
            builder.HasKey(a => a.AuthorId);
            builder.Property(a => a.AuthorName).IsRequired().HasMaxLength(100);

            builder.HasMany(a => a.Literatures)
                .WithOne(l => l.Author)
                .HasForeignKey(l => l.AuthorId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publishers");
            builder.HasKey(p => p.PublisherId);
            builder.Property(p => p.PublisherName).IsRequired().HasMaxLength(100);

            builder.HasMany(p => p.Literatures)
                .WithOne(l => l.Publisher)
                .HasForeignKey(l => l.PublisherId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class CaliberSpecConfiguration : IEntityTypeConfiguration<CaliberSpec>
    {
        public void Configure(EntityTypeBuilder<CaliberSpec> builder)
        {
            builder.ToTable("CaliberSpecs");
            builder.HasKey(c => c.CaliberSpecId);
            builder.Property(c => c.CaliberSpecName).IsRequired().HasMaxLength(100);

            builder.HasMany(c => c.MechanicalEquipments)
                .WithOne(me => me.CaliberSpec)
                .HasForeignKey(me => me.CaliberSpecId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.ToTable("Manufacturers");
            builder.HasKey(m => m.ManufacturerId);
            builder.Property(m => m.ManufacturerName).IsRequired().HasMaxLength(100);

            builder.HasMany(man => man.MechanicalEquipments)
                .WithOne(me => me.Manufacturer)
                .HasForeignKey(me => me.ManufacturerId)
                .IsRequired(false);
                //.OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class  ArtifactTypeConfiguration : IEntityTypeConfiguration<ArtifactType> 
    {    
        public void Configure(EntityTypeBuilder<ArtifactType> builder)    
        {        
            builder.ToTable("ArtifactTypes");        
            builder.HasKey(at => at.ArtifactTypeId);        
            builder.Property(at => at.ArtifactTypeName).IsRequired().HasMaxLength(100);

            builder.HasMany(at => at.Artifacts)
                .WithOne(a => a.ArtifactType)
                .HasForeignKey(a => a.ArtifactTypeId)
                .IsRequired();            
                //.OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class  EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
    {
        public void Configure(EntityTypeBuilder<EquipmentType> builder)
        {
            builder.ToTable("EquipmentTypes");
            builder.HasKey(et => et.EquipmentTypeId);
            builder.Property(et => et.EquipmentTypeName).IsRequired().HasMaxLength(100);

            builder.HasMany(et => et.Equipments)
                .WithOne(e => e.EquipmentType)
                .HasForeignKey(e => e.EquipmentTypeId)
                .IsRequired();
                //.OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class  InsigniaTypeConfiguration : IEntityTypeConfiguration<InsigniaType>
    {
        public void Configure(EntityTypeBuilder<InsigniaType> builder)
        {
            builder.ToTable("InsigniaTypes");
            builder.HasKey(it => it.InsigniaTypeId);
            builder.Property(it => it.InsigniaTypeName).IsRequired().HasMaxLength(75);

            builder.HasMany(it => it.Insignias)
                .WithOne(i => i.InsigniaType)
                .HasForeignKey(i => i.InsigniaTypeId)
                .IsRequired();
                //.OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class LiteratureTypeConfiguration : IEntityTypeConfiguration<LiteratureType>
    {
        public void Configure(EntityTypeBuilder<LiteratureType> builder)
        {
            builder.ToTable("LiteratureTypes");
            builder.HasKey(lt => lt.LiteratureTypeId);
            builder.Property(lt => lt.LiteratureTypeName).IsRequired().HasMaxLength(100);

            builder.HasMany(lt => lt.Literatures)
                .WithOne(l => l.LiteratureType)
                .HasForeignKey(l => l.LiteratureTypeId)
                .IsRequired();
                ////.OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class BindingTypeConfiguration : IEntityTypeConfiguration<BindingType>
    {
        public void Configure(EntityTypeBuilder<BindingType> builder)
        {
            builder.ToTable("BindingTypes");
            builder.HasKey(bt => bt.BindingTypeId);
            builder.Property(bt => bt.BindingTypeName).IsRequired().HasMaxLength(100);

            builder.HasMany(bt => bt.Literatures)
                .WithOne(l => l.BindingType)
                .HasForeignKey(l => l.BindingTypeId)
                .IsRequired(false);
                ////.OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class MechanicalEquipmentTypeConfiguration : IEntityTypeConfiguration<MechanicalEquipmentType>
    {
        public void Configure(EntityTypeBuilder<MechanicalEquipmentType> builder)
        {
            builder.ToTable("MechanicalEquipmentTypes");
            builder.HasKey(met => met.MechanicalEquipmentTypeId);
            builder.Property(met => met.MechanicalEquipmentTypeName).IsRequired().HasMaxLength(100);

            builder.HasMany(met => met.MechanicalEquipments)
                .WithOne(me => me.MechanicalEquipmentType)
                .HasForeignKey(me => me.MechanicalEquipmentTypeId)
                .IsRequired();
                ////.OnDelete(DeleteBehavior.Restrict);
        }
    }
}