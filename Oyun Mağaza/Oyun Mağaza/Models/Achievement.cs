using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Achievement
    {
        public Achievement()
        {
            UserAchievements = new HashSet<UserAchievement>();
        }

        [Key]
        public int AchievementId { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? Icon { get; set; }

        public bool IsHidden { get; set; }

        public double UnlockRate { get; set; } // Percentage of players who unlocked this achievement

        // Navigation Properties
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
    }
} 