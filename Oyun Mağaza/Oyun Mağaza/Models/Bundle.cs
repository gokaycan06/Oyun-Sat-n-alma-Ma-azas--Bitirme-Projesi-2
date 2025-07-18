using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class Bundle
    {
        public Bundle()
        {
            UserBundles = new HashSet<UserBundle>();
            Games = new HashSet<Game>();
            Discounts = new HashSet<BundleDiscount>();
        }

        [Key]
        public int BundleId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string? CoverImage { get; set; }

        // Navigation Properties
        public virtual ICollection<UserBundle> UserBundles { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<BundleDiscount> Discounts { get; set; }
    }
} 