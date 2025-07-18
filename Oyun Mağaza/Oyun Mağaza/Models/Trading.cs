using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class Trading
    {
        [Key]
        public int TradingId { get; set; }

        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; } = null!;

        public int ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; } = null!;

        public int GameOfferedId { get; set; }
        public virtual Game GameOffered { get; set; } = null!;

        public int GameRequestedId { get; set; }
        public virtual Game GameRequested { get; set; } = null!;

        public DateTime OfferDate { get; set; } = DateTime.Now;
        public DateTime? CompletionDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected, Cancelled

        public string? Message { get; set; }
    }
} 