using System;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class WishList
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        public DateTime AddedDate { get; set; } = DateTime.Now;

        public int Priority { get; set; } = 0; // 0: Normal, 1: High, 2: Must Have

        public string? Note { get; set; }
    }
} 