using System;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class GameReview
    {
        [Key]
        public int ReviewId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int Rating { get; set; } // 1-5 stars

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public int HelpfulCount { get; set; }

        public int NotHelpfulCount { get; set; }

        public bool IsRecommended { get; set; }

        public TimeSpan PlayTimeAtReview { get; set; }

        public bool IsVerifiedPurchase { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? LastEditDate { get; set; }
    }
} 