using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class CreditCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [MaxLength(19)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string NameOnCard { get; set; }

        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public int ExpiryYear { get; set; }

        [Required]
        [MaxLength(4)]
        public string Cvv { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 