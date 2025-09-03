using System.ComponentModel.DataAnnotations;

namespace MilitaryCollectiblesBackend.Models
{
    public class StorageArea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StorageAreaName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? StorageAreaNotes { get; set; }

        public ICollection<Literature> Literatures { get; set; } = new HashSet<Literature>();
        public ICollection<Insignia> Insignias { get; set; } = new HashSet<Insignia>();
        public ICollection<Artifact> Artifacts { get; set; } = new HashSet<Artifact>();
        public ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();
        public ICollection<MechanicalEquipment> MechanicalEquipments { get; set; } = new HashSet<MechanicalEquipment>();
    }
}
