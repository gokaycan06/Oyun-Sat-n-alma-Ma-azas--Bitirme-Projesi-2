using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models
{
    public class GameLanguage
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = string.Empty;

        public bool Interface { get; set; }
        public bool Audio { get; set; }
        public bool Subtitles { get; set; }
    }
} 