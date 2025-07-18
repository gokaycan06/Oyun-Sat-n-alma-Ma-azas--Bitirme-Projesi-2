using System;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class Update
    {
        [Key]
        public int UpdateId { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Version { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        public bool IsMandatory { get; set; }

        public long SizeInBytes { get; set; }

        public string? ChangeLog { get; set; }
    }
} 