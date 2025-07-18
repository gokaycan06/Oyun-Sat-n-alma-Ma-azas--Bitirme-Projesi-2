using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Category
    {
        public Category()
        {
            GameCategories = new HashSet<GameCategory>();
        }

        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public string? Icon { get; set; }

        // Navigation Properties
        public virtual ICollection<GameCategory> GameCategories { get; set; }
    }
} 