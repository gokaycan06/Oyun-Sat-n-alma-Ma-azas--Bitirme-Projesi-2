using System;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class UserAchievement
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; } = null!;

        public DateTime UnlockDate { get; set; }
    }
} 