using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.Models.DTOs;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
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

        // Sepete oyun ekleme
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Oyunun var olup olmadığını kontrol et
                var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == request.GameId);
                if (game == null)
                {
                    return Json(new { success = false, message = "Oyun bulunamadı." });
                }

                // Kullanıcının zaten bu oyuna sahip olup olmadığını kontrol et
                var userGame = await _context.UserGames
                    .FirstOrDefaultAsync(ug => ug.GameId == request.GameId && ug.UserId.ToString() == userId);
                if (userGame != null)
                {
                    return Json(new { success = false, message = "Bu oyuna zaten sahipsiniz." });
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

                // Oyunun zaten sepette olup olmadığını kontrol et
                if (existingOrder.OrderItems.Any(oi => oi.GameId == request.GameId))
                {
                    return Json(new { success = false, message = "Bu oyun zaten sepetinizde." });
                }

                // Sepete ekle
                var orderItem = new OrderItem
                {
                    OrderId = existingOrder.OrderId,
                    GameId = request.GameId,
                    Price = game.Price,
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
                    message = "Oyun sepete eklendi.",
                    cartCount = cartCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        // Sepetten oyun kaldırma
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Sepetteki oyunu bul
                var orderItem = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .FirstOrDefaultAsync(oi => oi.GameId == request.GameId && 
                                             oi.Order.UserId.ToString() == userId && 
                                             !oi.Order.IsCompleted);

                if (orderItem == null)
                {
                    return Json(new { success = false, message = "Oyun sepetinizde bulunamadı." });
                }

                // Sepetten kaldır
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                // Sepet sayısını güncelle
                var cartCount = await _context.OrderItems
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .CountAsync();

                return Json(new { 
                    success = true, 
                    message = "Oyun sepetten kaldırıldı.",
                    cartCount = cartCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        // Sepetten bundle kaldırma
        [HttpPost]
        public async Task<IActionResult> RemoveBundleFromCart([FromBody] RemoveBundleFromCartRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Bundle'ı sepette bul
                var orderItem = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .FirstOrDefaultAsync(oi => oi.BundleId == request.BundleId && 
                                             oi.Order.UserId.ToString() == userId && 
                                             !oi.Order.IsCompleted);

                if (orderItem == null)
                {
                    return Json(new { success = false, message = "Bundle sepetinizde bulunamadı." });
                }

                // Sepetten kaldır
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                // Sepet sayısını güncelle
                var cartCount = await _context.OrderItems
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .CountAsync();

                return Json(new { 
                    success = true, 
                    message = "Bundle sepetten kaldırıldı.",
                    cartCount = cartCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        // Sepetten DLC kaldırma
        [HttpPost]
        public async Task<IActionResult> RemoveDLCFromCart([FromBody] RemoveDLCFromCartRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // DLC'yi sepette bul
                var orderItem = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .FirstOrDefaultAsync(oi => oi.DLCId == request.DLCId && 
                                             oi.Order.UserId.ToString() == userId && 
                                             !oi.Order.IsCompleted);

                if (orderItem == null)
                {
                    return Json(new { success = false, message = "DLC sepetinizde bulunamadı." });
                }

                // Sepetten kaldır
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                // Sepet sayısını güncelle
                var cartCount = await _context.OrderItems
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .CountAsync();

                return Json(new { 
                    success = true, 
                    message = "DLC sepetten kaldırıldı.",
                    cartCount = cartCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        // Sepet içeriğini getirme
        [HttpPost]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                var cartItems = await _context.OrderItems
                    .Include(oi => oi.Game)
                    .Include(oi => oi.Bundle)
                        .ThenInclude(b => b.Games)
                    .Include(oi => oi.DLC)
                        .ThenInclude(d => d.Game)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .Select(oi => new
                    {
                        orderItemId = oi.OrderItemId,
                        itemType = oi.BundleId.HasValue ? "Bundle" : oi.DLCId.HasValue ? "DLC" : "Game",
                        gameId = oi.GameId,
                        bundleId = oi.BundleId,
                        dlcId = oi.DLCId,
                        title = oi.BundleId.HasValue ? oi.Bundle.Name : oi.DLCId.HasValue ? oi.DLC.Name : oi.Game.Title,
                        price = oi.Price,
                        quantity = oi.Quantity,
                        coverImage = oi.BundleId.HasValue
                            ? (oi.Bundle.Games.Any() ? "/games/" + oi.Bundle.Games.First().CoverImage : "/games/default-cover.jpg")
                            : oi.DLCId.HasValue
                            ? "/games/" + (oi.DLC.CoverImage ?? "default-cover.jpg")
                            : "/games/" + (oi.Game.CoverImage ?? "default-cover.jpg"),
                        gameCount = oi.BundleId.HasValue ? oi.Bundle.Games.Count : 1
                    })
                    .ToListAsync();

                var totalPrice = cartItems.Sum(item => item.price * item.quantity);
                var itemCount = cartItems.Count;

                return Json(new { 
                    success = true, 
                    items = cartItems,
                    totalPrice = totalPrice,
                    itemCount = itemCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Sepet bilgileri alınamadı." });
            }
        }

        // Sepet sayfasını görüntüleme
        public async Task<IActionResult> Index()
        {
            SetViewBagValues();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _context.OrderItems
                .Include(oi => oi.Game)
                    .ThenInclude(g => g.Developer)
                .Include(oi => oi.Game)
                    .ThenInclude(g => g.Publisher)
                .Include(oi => oi.Game)
                    .ThenInclude(g => g.GameCategories)
                        .ThenInclude(gc => gc.Category)
                .Include(oi => oi.Bundle)
                    .ThenInclude(b => b.Games)
                .Include(oi => oi.DLC)
                    .ThenInclude(d => d.Game)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                .ToListAsync();

            var totalPrice = cartItems.Sum(item => item.Price * item.Quantity);

            ViewBag.CartItems = cartItems;
            ViewBag.TotalPrice = totalPrice;

            return View();
        }

        // Sepeti temizleme
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                var orderItems = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .ToListAsync();

                _context.OrderItems.RemoveRange(orderItems);
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = "Sepet temizlendi.",
                    cartCount = 0
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        // Sepet sayısını getirme
        [HttpPost]
        public async Task<IActionResult> GetCartCount()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, count = 0 });
                }

                var cartCount = await _context.OrderItems
                    .Where(oi => oi.Order.UserId.ToString() == userId && !oi.Order.IsCompleted)
                    .CountAsync();

                return Json(new { success = true, count = cartCount });
            }
            catch (Exception)
            {
                return Json(new { success = false, count = 0 });
            }
        }

        // Bundle'ı sepete ekleme
        [HttpPost]
        public async Task<IActionResult> AddBundleToCart([FromBody] AddBundleToCartRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Bundle'ın var olup olmadığını kontrol et
                var bundle = await _context.Bundles
                    .Include(b => b.Games)
                    .FirstOrDefaultAsync(b => b.BundleId == request.BundleId);
                
                if (bundle == null)
                {
                    return Json(new { success = false, message = "Bundle bulunamadı." });
                }

                // Kullanıcının zaten bu bundle'daki oyunlara sahip olup olmadığını kontrol et
                var userGames = await _context.UserGames
                    .Where(ug => ug.UserId.ToString() == userId)
                    .Select(ug => ug.GameId)
                    .ToListAsync();

                var bundleGameIds = bundle.Games.Select(g => g.GameId).ToList();
                var alreadyOwnedGames = bundleGameIds.Intersect(userGames).ToList();

                if (alreadyOwnedGames.Any())
                {
                    var ownedGameNames = await _context.Games
                        .Where(g => alreadyOwnedGames.Contains(g.GameId))
                        .Select(g => g.Title)
                        .ToListAsync();
                    
                    return Json(new { 
                        success = false, 
                        message = $"Bu bundle'daki bazı oyunlara zaten sahipsiniz: {string.Join(", ", ownedGameNames)}" 
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

                // Bundle'ın zaten sepette olup olmadığını kontrol et
                if (existingOrder.OrderItems.Any(oi => oi.BundleId == request.BundleId))
                {
                    return Json(new { 
                        success = false, 
                        message = "Bu bundle zaten sepetinizde." 
                    });
                }

                // Bundle'ı sepete ekle (tek bir OrderItem olarak)
                var orderItem = new OrderItem
                {
                    OrderId = existingOrder.OrderId,
                    BundleId = request.BundleId,
                    Price = bundle.Price,
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
                    message = $"Bundle sepete eklendi.",
                    cartCount = cartCount
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        // DLC'yi sepete ekleme
        [HttpPost]
        public async Task<IActionResult> AddDLCToCart([FromBody] AddDLCToCartRequest request)
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

        // Satın alma işlemi
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Lütfen giriş yapın." });
                }

                // Kullanıcının sepetini bul
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Game)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Bundle)
                            .ThenInclude(b => b.Games)
                    .FirstOrDefaultAsync(o => o.UserId.ToString() == userId && !o.IsCompleted);

                if (order == null || !order.OrderItems.Any())
                {
                    return Json(new { success = false, message = "Sepetinizde ürün bulunmuyor." });
                }

                // Toplam tutarı hesapla
                var totalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

                // Kullanıcının bakiyesini kontrol et
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı." });
                }

                if (user.WalletBalance < totalAmount)
                {
                    return Json(new { success = false, message = $"Yetersiz bakiye. Gerekli: ₺{totalAmount:F2}, Mevcut: ₺{user.WalletBalance:F2}" });
                }

                // Bakiyeyi düş
                user.WalletBalance -= totalAmount;

                // Siparişi tamamla
                order.IsCompleted = true;

                // Kullanıcının oyunlarını ekle
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem.GameId.HasValue)
                    {
                        // Tek oyun satın alma
                        var userGame = new UserGame
                        {
                            UserId = int.Parse(userId),
                            GameId = orderItem.GameId.Value,
                            PurchaseDate = DateTime.Now
                        };
                        _context.UserGames.Add(userGame);
                    }
                    else if (orderItem.BundleId.HasValue)
                    {
                        // Bundle satın alma
                        var bundle = await _context.Bundles
                            .Include(b => b.Games)
                            .FirstOrDefaultAsync(b => b.BundleId == orderItem.BundleId.Value);

                        if (bundle != null)
                        {
                            foreach (var game in bundle.Games)
                            {
                                var userGame = new UserGame
                                {
                                    UserId = int.Parse(userId),
                                    GameId = game.GameId,
                                    PurchaseDate = DateTime.Now
                                };
                                _context.UserGames.Add(userGame);
                            }
                        }
                    }
                    else if (orderItem.DLCId.HasValue)
                    {
                        // DLC satın alma
                        var userDLC = new UserDLC
                        {
                            UserId = int.Parse(userId),
                            DLCId = orderItem.DLCId.Value,
                            PurchaseDate = DateTime.Now,
                            IsInstalled = false
                        };
                        _context.UserDLCs.Add(userDLC);
                    }
                }

                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = $"Satın alma işlemi başarılı! {order.OrderItems.Count} ürün kütüphanenize eklendi.",
                    cartCount = 0
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Satın alma işlemi sırasında bir hata oluştu." });
            }
        }
    }
} 