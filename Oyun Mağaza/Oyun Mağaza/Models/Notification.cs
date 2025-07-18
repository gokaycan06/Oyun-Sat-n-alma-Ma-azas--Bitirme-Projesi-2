using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // "message", "friend_request", "system"
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? RelatedId { get; set; } // Mesaj ID'si veya arkadaşlık isteği ID'si
        
        // Navigation property
        public virtual User User { get; set; }
    }
} 