using System;
using System.Collections.Generic;

namespace Oyun_Mağaza.ViewModels
{
    public class BundleDetailViewModel
    {
        public int BundleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal CurrentPrice { get; set; }
        public string CoverImage { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDiscounted { get; set; }
        public int GameCount { get; set; }
        public List<BundleGameViewModel> Games { get; set; } = new List<BundleGameViewModel>();
    }

    public class BundleGameViewModel
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CoverImage { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string DeveloperName { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Screenshots { get; set; } = new List<string>();
    }
} 