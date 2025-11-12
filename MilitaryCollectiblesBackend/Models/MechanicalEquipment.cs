using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryCollectiblesBackend.Models
{
    public class MechanicalEquipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool Availability { get; set; } = true;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } = 0.00m;

        [Required]
        public int MechanicalEquipmentTypeId { get; set; }

        [MaxLength(50)]
        public int? CaliberSpecId { get; set; }

        [MaxLength(100)]
        public string? VehicleModel { get; set; }

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        public int? ManufacturerId { get; set; }

        public int? EraId { get; set; }

        public int? OriginId { get; set; }

        public int? MaterialId { get; set; }

        [MaxLength(500)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(200)]
        public int? StorageArea { get; set; }
        public StorageArea? StorageAreaDetails { get; set; }
        public MechanicalEquipmentType MechanicalEquipmentType { get; set; } = new MechanicalEquipmentType();
        public CaliberSpec? CaliberSpec { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public Era? Era { get; set; }
        public Origin? Origin { get; set; }
        public Material? Material { get; set; }
    }
}
