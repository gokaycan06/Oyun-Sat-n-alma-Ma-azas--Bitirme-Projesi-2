using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    public class BundleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BundleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ViewBag değerlerini set eden metod
        private void SetViewBagValues()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
                {
                    var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                    if (user != null)
                    {
                        ViewBag.UserDisplayName = user.DisplayName ?? user.Username;
                        ViewBag.WalletBalance = user.WalletBalance.ToString("F2");
                        ViewBag.UserBalance = user.WalletBalance;
                    }
                }
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            SetViewBagValues();
            
            var bundle = await _context.Bundles
                .Include(b => b.Games)
                .ThenInclude(g => g.Developer)
                .Include(b => b.Games)
                .ThenInclude(g => g.Publisher)
                .Include(b => b.Games)
                .ThenInclude(g => g.GameCategories)
                .ThenInclude(gc => gc.Category)
                .Include(b => b.Games)
                .ThenInclude(g => g.Screenshots)
                .FirstOrDefaultAsync(b => b.BundleId == id);

            if (bundle == null)
            {
                return NotFound();
            }

            var bundleDetailViewModel = new BundleDetailViewModel
            {
                BundleId = bundle.BundleId,
                Name = bundle.Name,
                Description = bundle.Description,
                Price = bundle.Price,
                DiscountPercentage = bundle.DiscountPercentage,
                CurrentPrice = bundle.Price * (100 - bundle.DiscountPercentage) / 100,
                CoverImage = bundle.Games.FirstOrDefault() != null && !string.IsNullOrEmpty(bundle.Games.FirstOrDefault().CoverImage) 
                    ? "/games/" + bundle.Games.FirstOrDefault().CoverImage 
                    : "/games/cs2-cover.jpg",
                StartDate = bundle.StartDate,
                EndDate = bundle.EndDate,
                IsActive = bundle.IsActive,
                IsDiscounted = bundle.DiscountPercentage > 0,
                GameCount = bundle.Games.Count,
                Games = bundle.Games.Select(g => new BundleGameViewModel
                {
                    GameId = g.GameId,
                    Title = g.Title,
                    Description = g.Description,
                    Price = g.Price,
                    CoverImage = !string.IsNullOrEmpty(g.CoverImage) ? "/games/" + g.CoverImage : "",
                    ReleaseDate = g.ReleaseDate,
                    DeveloperName = g.Developer?.Name ?? "",
                    PublisherName = g.Publisher?.Name ?? "",
                    Categories = g.GameCategories.Select(gc => gc.Category.Name).ToList(),
                    Screenshots = g.Screenshots.Select(s => s.ImageUrl).ToList()
                }).ToList()
            };

            return View(bundleDetailViewModel);
        }
    }
} 