using System.ComponentModel.DataAnnotations;

namespace MilitaryCollectiblesBackend.Models
{
    public class LiteratureSeries
    {
        [Key]
        public int SeriesId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SeriesName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Literature> Literatures { get; set; } = new HashSet<Literature>(); // HashSet for uniqueness and fast lookup, List when ordering and duplicates allowed
    }
}
