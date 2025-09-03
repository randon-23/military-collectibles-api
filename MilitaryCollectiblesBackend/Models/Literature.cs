using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryCollectiblesBackend.Models
{
    public class Literature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public bool Availability { get; set; } = true;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } = 0.00m;

        [MaxLength(200)]
        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }

        [MaxLength(200)]
        public string? ISBN { get; set; }

        [Required]
        [MaxLength(50)]
        public string LiteratureType { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string BindingType { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(200)]
        public int? StorageArea { get; set; }

        public int? SeriesId { get; set; }

        public StorageArea? StorageAreaDetails { get; set; }
        public LiteratureSeries? LiteratureSeries { get; set; }
    }
}
