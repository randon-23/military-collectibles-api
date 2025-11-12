using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryCollectiblesBackend.Models
{
    public class Origin
    {
        [Key]
        public int OriginId { get; set; }
        [Required]
        [MaxLength(100)]
        public string OriginName { get; set; } = string.Empty;
        public ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
        public ICollection<Insignia> Insignias { get; set; } = new HashSet<Insignia>();
        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
        public ICollection<Artifact> Artifacts { get; set; } = new HashSet<Artifact>();
    }

    public class Era
    {
        [Key]
        public int EraId { get; set; }
        [Required]
        [MaxLength(100)]
        public string EraName { get; set; } = string.Empty;
        public ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
        public ICollection<Insignia> Insignias { get; set; } = new HashSet<Insignia>();
        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
        public ICollection<Artifact> Artifacts { get; set; } = new HashSet<Artifact>();
    }

    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        [Required]
        [MaxLength(100)]
        public string MaterialName { get; set; } = string.Empty;
        public ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
        public ICollection<Insignia> Insignias { get; set; } = new HashSet<Insignia>();
        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
    }

    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; } = string.Empty;
        public ICollection<Literature> Literatures { get; set; } = new HashSet<Literature>();
    }

    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        [Required]
        [MaxLength(100)]
        public string PublisherName { get; set; } = string.Empty;
        public ICollection<Literature> Literatures { get; set; } = new HashSet<Literature>();
    }

    public class CaliberSpec
    {
        [Key]
        public int CaliberSpecId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CaliberSpecName { get; set; } = string.Empty;
        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
    }

    public class Manufacturer
    {
        [Key]
        public int ManufacturerId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ManufacturerName { get; set; } = string.Empty;
        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
    }

    public class ArtifactType
    {
        [Key]
        public int ArtifactTypeId { get; set; }
        [Required]
        [MaxLength(75)]
        public string ArtifactTypeName { get; set; } = string.Empty;
        public ICollection<Artifact> Artifacts { get; set; } = new HashSet<Artifact>();
    }

    public class EquipmentType
    {
        [Key]
        public int EquipmentTypeId { get; set; }
        [Required]
        [MaxLength(75)]
        public string EquipmentTypeName { get; set; } = string.Empty;

        public ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
    }

    public class InsigniaType
    {
        [Key]
        public int InsigniaTypeId { get; set; }
        [Required]
        [MaxLength(75)]
        public string InsigniaTypeName { get; set; } = string.Empty;

        public ICollection<Insignia> Insignias { get; set; } = new HashSet<Insignia>();
    }

    public class LiteratureType
    {
        [Key]
        public int LiteratureTypeId { get; set; }
        [Required]
        [MaxLength(75)]
        public string LiteratureTypeName { get; set; } = string.Empty;
        public ICollection<Literature> Literatures { get; set; } = new HashSet<Literature>();
    }

    public class BindingType
    {
        [Key]
        public int BindingTypeId { get; set; }
        [Required]
        [MaxLength(75)]
        public string BindingTypeName { get; set; } = string.Empty;

        public ICollection<Literature> Literatures { get; set; } = new HashSet<Literature>();
    }

    public class  MechanicalEquipmentType
    {
        [Key]
        public int MechanicalEquipmentTypeId { get; set; }
        [Required]
        [MaxLength(75)]
        public string MechanicalEquipmentTypeName { get; set; } = string.Empty;

        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
    }
}
