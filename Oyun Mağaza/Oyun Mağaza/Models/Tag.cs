using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Tag
    {
        public Tag()
        {
            GameTags = new HashSet<GameTag>();
        }

        [Key]
        public int TagId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        // Navigation Properties
        public virtual ICollection<GameTag> GameTags { get; set; }
    }
} 