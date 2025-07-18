using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        // Oyun için (nullable çünkü bundle da olabilir)
        public int? GameId { get; set; }
        [ForeignKey("GameId")]
        public Game? Game { get; set; }

        // Bundle için (nullable çünkü oyun da olabilir)
        public int? BundleId { get; set; }
        [ForeignKey("BundleId")]
        public Bundle? Bundle { get; set; }

        // DLC için (nullable çünkü oyun veya bundle da olabilir)
        public int? DLCId { get; set; }
        [ForeignKey("DLCId")]
        public DLC? DLC { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        // Item tipini belirlemek için
        public string ItemType => BundleId.HasValue ? "Bundle" : DLCId.HasValue ? "DLC" : "Game";
    }
} 