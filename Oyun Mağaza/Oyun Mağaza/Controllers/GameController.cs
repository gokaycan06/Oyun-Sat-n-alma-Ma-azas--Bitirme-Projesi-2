using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.ViewModels;
using System.Security.Claims;
using System.Linq;

namespace Oyun_Mağaza.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
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
                        ViewBag.UserProfilePicture = !string.IsNullOrEmpty(user.ProfilePicture) ? user.ProfilePicture : "/images/avatars/default-avatar.jpg";
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Oyunun zaten istek listesinde olup olmadığını kontrol et
                var existingWishlist = await _context.WishLists
                    .FirstOrDefaultAsync(w => w.GameId == id && w.UserId.ToString() == userId);

                if (existingWishlist != null)
                {
                    return Json(new { success = false, message = "Bu oyun zaten istek listenizde." });
                }

                // İstek listesine ekle
                var wishlist = new WishList
                {
                    GameId = id,
                    UserId = int.Parse(userId),
                    AddedDate = DateTime.Now
                };

                _context.WishLists.Add(wishlist);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Oyunun zaten sepette olup olmadığını kontrol et
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.UserId.ToString() == userId && !o.IsCompleted);

                if (existingOrder?.OrderItems.Any(oi => oi.GameId == id) == true)
                {
                    return Json(new { success = false, message = "Bu oyun zaten sepetinizde." });
                }

                // Yeni sipariş oluştur veya var olan siparişe ekle
                if (existingOrder == null)
                {
                    existingOrder = new Order
                    {
                        UserId = int.Parse(userId),
                        OrderDate = DateTime.Now,
                        IsCompleted = false
                    };
                    _context.Orders.Add(existingOrder);
                    await _context.SaveChangesAsync();
                }

                // Sipariş öğesini ekle
                var orderItem = new OrderItem
                {
                    OrderId = existingOrder.OrderId,
                    GameId = id,
                    Quantity = 1
                };

                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            SetViewBagValues();
            
            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.GameTags)
                    .ThenInclude(gt => gt.Tag)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                
                .Include(g => g.Screenshots)
                .Include(g => g.Videos)
                .Include(g => g.GameLanguages)
                .Include(g => g.SystemRequirements)
                .Include(g => g.DLCs)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.GameId == id && ug.UserId.ToString() == userId);

            // Sepette olup olmadığını kontrol et
            var isInCart = await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.GameId == id && 
                               oi.Order.UserId.ToString() == userId && 
                               !oi.Order.IsCompleted);

            // İstek listesinde olup olmadığını kontrol et
            var isWishlisted = await _context.WishLists
                .AnyAsync(w => w.GameId == id && w.UserId.ToString() == userId);

            var viewModel = new GameDetailViewModel
            {
                Game = game,
                Screenshots = game.Screenshots?.ToList() ?? new List<Screenshot>(),
                Reviews = game.Reviews?.ToList() ?? new List<GameReview>(),
                DLCs = game.DLCs?.ToList() ?? new List<DLC>(),
                IsInCart = isInCart,
                IsOwned = userGame != null,
                IsWishlisted = isWishlisted,
                IsInstalled = userGame?.IsInstalled ?? false,
                PlayTime = userGame?.PlayTime.Hours ?? 0,
                AverageRating = game.Reviews?.Any() == true ? game.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = game.Reviews?.Count ?? 0
            };

            return View(viewModel);
        }
    }
} 