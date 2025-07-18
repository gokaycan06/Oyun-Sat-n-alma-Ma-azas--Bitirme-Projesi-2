using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Video
    {
        [Key]
        public int VideoId { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Url { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
} 