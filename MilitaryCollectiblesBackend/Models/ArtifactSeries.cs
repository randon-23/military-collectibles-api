using System.ComponentModel.DataAnnotations;

namespace MilitaryCollectiblesBackend.Models
{
    public class ArtifactSeries
    {
        [Key]
        public int SeriesId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SeriesName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
        public ICollection<Artifact> Artifacts { get; set; } = new HashSet<Artifact>(); //hashset for uniqueness and fast lookup, list when ordering and duplicates allwoed
    }
}
