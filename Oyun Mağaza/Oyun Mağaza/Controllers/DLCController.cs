using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.Models.DTOs;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    public class DLCController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DLCController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DLC detay sayfası
        public async Task<IActionResult> Details(int id)
        {
            var dlc = await _context.DLCs
                .Include(d => d.Game)
                    .ThenInclude(g => g.Developer)
                .Include(d => d.Game)
                    .ThenInclude(g => g.Publisher)
                .Include(d => d.Game)
                    .ThenInclude(g => g.GameCategories)
                        .ThenInclude(gc => gc.Category)
                .FirstOrDefaultAsync(d => d.DLCId == id);

            if (dlc == null)
            {
                return NotFound();
            }

            // Kullanıcının bu DLC'ye sahip olup olmadığını kontrol et
            bool userOwnsDLC = false;
            bool userOwnsGame = false;

            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    userOwnsDLC = await _context.UserDLCs
                        .AnyAsync(ud => ud.DLCId == id && ud.UserId.ToString() == userId);

                    userOwnsGame = await _context.UserGames
                        .AnyAsync(ug => ug.GameId == dlc.GameId && ug.UserId.ToString() == userId);
                }
            }

            // Aynı oyuna ait diğer DLC'leri getir
            var relatedDLCs = await _context.DLCs
                .Where(d => d.GameId == dlc.GameId && d.DLCId != id)
                .Take(4)
                .ToListAsync();

            ViewBag.UserOwnsDLC = userOwnsDLC;
            ViewBag.UserOwnsGame = userOwnsGame;
            ViewBag.RelatedDLCs = relatedDLCs;

            return View(dlc);
        }

        // DLC'yi sepete ekleme
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddDLCToCartRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // DLC'nin var olup olmadığını kontrol et
                var dlc = await _context.DLCs
                    .Include(d => d.Game)
                    .FirstOrDefaultAsync(d => d.DLCId == request.DLCId);
                
                if (dlc == null)
                {
                    return Json(new { success = false, message = "DLC bulunamadı." });
                }

                // Kullanıcının ana oyuna sahip olup olmadığını kontrol et
                var userOwnsGame = await _context.UserGames
                    .AnyAsync(ug => ug.GameId == dlc.GameId && ug.UserId.ToString() == userId);

                if (!userOwnsGame)
                {
                    return Json(new { 
                        success = false, 
                        message = "Bu DLC'yi satın almak için önce ana oyuna sahip olmalısınız." 
                    });
                }

                // Kullanıcının zaten bu DLC'ye sahip olup olmadığını kontrol et
                var userOwnsDLC = await _context.UserDLCs
                    .AnyAsync(ud => ud.DLCId == request.DLCId && ud.UserId.ToString() == userId);

                if (userOwnsDLC)
                {
                    return Json(new { 
                        success = false, 
                        message = "Bu DLC'ye zaten sahipsiniz." 
                    });
                }

                // Aktif sepeti bul veya yeni oluştur
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.UserId.ToString() == userId && !o.IsCompleted);

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

                // DLC'nin zaten sepette olup olmadığını kontrol et
                if (existingOrder.OrderItems.Any(oi => oi.DLCId == request.DLCId))
                {
                    return Json(new { 
                        success = false, 
                        message = "Bu DLC zaten sepetinizde." 
                    });
                }

                // DLC'yi sepete ekle
                var orderItem = new OrderItem
                {
                    OrderId = existingOrder.OrderId,
                    DLCId = request.DLCId,
                    Price = dlc.Price,
                    Quantity = 1
                };

                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                // Sepet sayısını güncelle
                var cartCount = await _context.OrderItems
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .CountAsync();

                return Json(new { 
                    success = true, 
                    message = $"DLC sepete eklendi.",
                    cartCount = cartCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }
    }
} 