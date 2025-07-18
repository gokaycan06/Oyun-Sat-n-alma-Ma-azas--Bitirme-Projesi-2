using System;
using System.Collections.Generic;

namespace Oyun_Mağaza.ViewModels
{
    public class GameCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CoverUrl { get; set; }
        public string VideoUrl { get; set; } = "";
        public List<string> ScreenshotUrls { get; set; } = new List<string>();
        public string Developer { get; set; } = "";
        public string Publisher { get; set; } = "";
        public DateTime? ReleaseDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
} 