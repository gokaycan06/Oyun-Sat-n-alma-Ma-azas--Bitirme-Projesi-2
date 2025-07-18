using System;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Screenshot
    {
        [Key]
        public int ScreenshotId { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string Caption { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        public int LikeCount { get; set; }
    }
} 