namespace Oyun_Mağaza.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string BackgroundImage { get; set; } = string.Empty;
        public int GameCount { get; set; }
    }
} 