using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class Game
    {
        public Game()
        {
            UserGames = new HashSet<UserGame>();
            GameCategories = new List<GameCategory>();
            GameTags = new HashSet<GameTag>();
            GameLanguages = new HashSet<GameLanguage>();
            SystemRequirements = new HashSet<SystemRequirement>();
            DLCs = new HashSet<DLC>();
            Screenshots = new HashSet<Screenshot>();
            Reviews = new HashSet<GameReview>();
            Achievements = new HashSet<Achievement>();
            Updates = new HashSet<Update>();
            Bundles = new HashSet<Bundle>();
            WishLists = new HashSet<WishList>();
            Videos = new HashSet<Video>();
        }

        [Key]
        public int GameId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? CoverImage { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int DeveloperId { get; set; }
        public virtual Developer Developer { get; set; } = null!;

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public int MinimumAge { get; set; }

        // Navigation Properties
        public virtual ICollection<UserGame> UserGames { get; set; }
        public virtual List<GameCategory> GameCategories { get; set; }
        public virtual ICollection<GameTag> GameTags { get; set; }
        public virtual ICollection<GameLanguage> GameLanguages { get; set; }
        public virtual ICollection<SystemRequirement> SystemRequirements { get; set; }
        public virtual ICollection<DLC> DLCs { get; set; }
        public virtual ICollection<Screenshot> Screenshots { get; set; }
        public virtual ICollection<GameReview> Reviews { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Update> Updates { get; set; }
        public virtual ICollection<Bundle> Bundles { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
} 