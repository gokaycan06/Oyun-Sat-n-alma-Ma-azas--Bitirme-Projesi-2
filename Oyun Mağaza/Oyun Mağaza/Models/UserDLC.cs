using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class UserDLC
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int DLCId { get; set; }
        public virtual DLC DLC { get; set; } = null!;

        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        public bool IsInstalled { get; set; }
    }
} 