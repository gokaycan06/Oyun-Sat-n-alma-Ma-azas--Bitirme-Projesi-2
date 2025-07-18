namespace Oyun_Mağaza.ViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? DiscountPercentage { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Screenshots { get; set; } = new List<string>();
        public string Description { get; set; }
        public List<string> Platforms { get; set; } = new List<string>();
        public string Badge { get; set; }

        public decimal CurrentPrice => DiscountPercentage.HasValue
            ? Price * (100 - DiscountPercentage.Value) / 100
            : Price;
    }
} 