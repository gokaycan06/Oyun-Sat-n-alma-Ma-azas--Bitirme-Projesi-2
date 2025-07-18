using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class DLC
    {
        public DLC()
        {
            UserDLCs = new HashSet<UserDLC>();
        }

        [Key]
        public int DLCId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        public string? CoverImage { get; set; }

        // Navigation Properties
        public virtual ICollection<UserDLC> UserDLCs { get; set; }
    }
} 