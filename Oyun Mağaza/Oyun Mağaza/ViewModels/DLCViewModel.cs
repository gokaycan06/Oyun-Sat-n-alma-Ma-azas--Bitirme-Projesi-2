namespace Oyun_Mağaza.ViewModels
{
    public class DLCViewModel
    {
        public int DLCId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; } = string.Empty;
        public string GameName { get; set; } = string.Empty;
        public int GameId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsInstalled { get; set; }
        public bool IsOwned { get; set; }
    }
} 