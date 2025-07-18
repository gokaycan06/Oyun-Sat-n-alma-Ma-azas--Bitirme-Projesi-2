using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class UserGame
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        public TimeSpan PlayTime { get; set; }

        public DateTime LastPlayed { get; set; }

        public bool IsInstalled { get; set; }

        public string? InstallPath { get; set; }
    }
} 