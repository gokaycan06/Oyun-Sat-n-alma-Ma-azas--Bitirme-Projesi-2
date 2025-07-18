using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oyun_Mağaza.Models
{
    public class GameCategory
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; } = null!;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
} 