using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryCollectiblesBackend.Models
{
    public class Insignia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool Availability { get; set; } = true;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } = 0.00m;

        [Required]
        public int InsigniaTypeId { get; set; }

        [Required]
        public bool PartOfSet { get; set; } = false;

        public int? OriginId { get; set; }

        public int? EraId { get; set; }

        public int? MaterialId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(200)]
        public int? StorageArea { get; set; }

        public int? SeriesId { get; set; }

        public StorageArea? StorageAreaDetails { get; set; }
        public InsigniaSeries? InsigniaSeries { get; set; }
        public InsigniaType InsigniaType { get; set; } = new InsigniaType();
        public Origin? Origin { get; set; }
        public Era? Era { get; set; }
        public Material? Material { get; set; }
    }
}
