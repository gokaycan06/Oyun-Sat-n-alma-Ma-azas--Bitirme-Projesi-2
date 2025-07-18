using System;
using System.Collections.Generic;
using Oyun_Mağaza.Models;

namespace Oyun_Mağaza.ViewModels
{
    public class StoreIndexViewModel
    {
        public List<GameCardViewModel> SliderGames { get; set; } = new List<GameCardViewModel>();
        public List<GameCardViewModel> FeaturedGames { get; set; } = new List<GameCardViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<BundleViewModel> Bundles { get; set; } = new List<BundleViewModel>();
        public BrowseGamesViewModel BrowseGames { get; set; } = new BrowseGamesViewModel();
    }

    public class BundleViewModel
    {
        public int BundleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal CurrentPrice => Price * (100 - DiscountPercentage) / 100;
        public string CoverImage { get; set; } = string.Empty;
        public List<string> GameNames { get; set; } = new List<string>();
        public List<string> GameImages { get; set; } = new List<string>();
        public int GameCount { get; set; }
        public bool IsDiscounted => DiscountPercentage > 0;
        public DateTime? PurchaseDate { get; set; }
    }

    public class BrowseGamesViewModel
    {
        public List<GameCardViewModel> AllGames { get; set; } = new List<GameCardViewModel>();
        public List<GameCardViewModel> TopSellers { get; set; } = new List<GameCardViewModel>();
        public int SelectedCategoryId { get; set; } = 0;
        public string SelectedTab { get; set; } = "all";
    }

    public class CategoryFilterModel
    {
        public int CategoryId { get; set; }
    }
} 