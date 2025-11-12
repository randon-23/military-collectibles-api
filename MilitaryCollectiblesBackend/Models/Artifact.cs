using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryCollectiblesBackend.Models
{
    public class Artifact
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
        public int ArtifactTypeId { get; set; }

        public int? OriginId { get; set; }

        public int? EraId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(200)]
        public int? StorageArea { get; set; }

        public int? SeriesId { get; set; }

        public StorageArea? StorageAreaDetails { get; set; }
        public ArtifactSeries? ArtifactSeries { get; set; }
        public ArtifactType ArtifactType { get; set; } = new ArtifactType();
        public Origin? Origin { get; set; }
        public Era? Era { get; set; }
    }
}
