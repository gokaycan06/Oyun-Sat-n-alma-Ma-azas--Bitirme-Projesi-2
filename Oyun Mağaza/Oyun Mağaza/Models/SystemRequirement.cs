using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class SystemRequirement
    {
        [Key]
        public int SystemRequirementId { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        [Required]
        public string Type { get; set; } = "Minimum"; // Minimum veya Recommended

        [Required]
        public string OS { get; set; } = string.Empty;

        [Required]
        public string Processor { get; set; } = string.Empty;

        [Required]
        public string Memory { get; set; } = string.Empty;

        [Required]
        public string Graphics { get; set; } = string.Empty;

        [Required]
        public string Storage { get; set; } = string.Empty;

        public string? AdditionalNotes { get; set; }
    }
} 