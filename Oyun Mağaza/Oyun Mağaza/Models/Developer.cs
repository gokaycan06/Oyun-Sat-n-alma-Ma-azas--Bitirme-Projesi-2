using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Developer
    {
        public Developer()
        {
            Games = new HashSet<Game>();
        }

        [Key]
        public int DeveloperId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        public int FoundedYear { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public string? Website { get; set; }

        public string? LogoUrl { get; set; }

        public bool IsVerified { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<Game> Games { get; set; }
    }
} 