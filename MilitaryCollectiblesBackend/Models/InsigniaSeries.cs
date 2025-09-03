using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryCollectiblesBackend.Models
{
    public class InsigniaSeries
    {
        [Key]
        public int SeriesId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SeriesName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Insignia> Insignias { get; set; } = new HashSet<Insignia>(); // HashSet for uniqueness and fast lookup, List when ordering and duplicates allowed
    }
}
