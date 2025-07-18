using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        
        // Navigation properties
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
} 