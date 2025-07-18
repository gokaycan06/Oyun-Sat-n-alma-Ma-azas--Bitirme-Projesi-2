using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class User
    {
        public User()
        {
            UserGames = new HashSet<UserGame>();
            UserDLCs = new HashSet<UserDLC>();
            UserBundles = new HashSet<UserBundle>();
            WishListItems = new HashSet<WishList>();
            Reviews = new HashSet<GameReview>();
            Achievements = new HashSet<UserAchievement>();
            TradingsSent = new HashSet<Trading>();
            TradingsReceived = new HashSet<Trading>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string ProfilePicture { get; set; } = "default-avatar.png";

        [StringLength(50)]
        public string? DisplayName { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public DateTime? BirthDate { get; set; }

        public decimal WalletBalance { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool EmailConfirmed { get; set; }

        public bool IsAdmin { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<UserGame> UserGames { get; set; }
        public virtual ICollection<UserDLC> UserDLCs { get; set; }
        public virtual ICollection<UserBundle> UserBundles { get; set; }
        public virtual ICollection<WishList> WishListItems { get; set; }
        public virtual ICollection<GameReview> Reviews { get; set; }
        public virtual ICollection<UserAchievement> Achievements { get; set; }
        public virtual ICollection<Trading> TradingsSent { get; set; }
        public virtual ICollection<Trading> TradingsReceived { get; set; }
    }
} 