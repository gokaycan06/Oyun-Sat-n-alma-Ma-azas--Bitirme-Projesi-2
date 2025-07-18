using Oyun_Mağaza.Models;

namespace Oyun_Mağaza.ViewModels
{
    public class GameDetailViewModel
    {
        public Game Game { get; set; }
        public List<Screenshot> Screenshots { get; set; }
        public List<GameReview> Reviews { get; set; }
        public List<DLC> DLCs { get; set; }
        public bool IsInCart { get; set; }
        public bool IsOwned { get; set; }
        public bool IsWishlisted { get; set; }
        public bool IsInstalled { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int? PlayTime { get; set; }
    }
} 