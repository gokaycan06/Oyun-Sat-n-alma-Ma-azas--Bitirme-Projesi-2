using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.ViewModels;
using Oyun_Mağaza.Models.DTOs;
using System.Security.Claims;
using System.IO; // Added for FileStream
using System; // Added for DateTime, TimeSpan

namespace Oyun_Mağaza.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
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
                        ViewBag.UserBalance = user.WalletBalance; // UserBalance değerini de set ediyoruz
                        ViewBag.UserProfilePicture = !string.IsNullOrEmpty(user.ProfilePicture) ? user.ProfilePicture : "/images/avatars/default-avatar.jpg";
                    }
                }
            }
        }

        public async Task<IActionResult> Index()
        {
            SetViewBagValues();
            
            var model = new StoreIndexViewModel();

            // Slider oyunları - Hem öne çıkan hem de rastgele oyunları karıştır
            var featuredGames = await _context.Games
                .Where(g => g.IsActive && g.IsFeatured)
                .Include(g => g.Screenshots)
                .Include(g => g.Videos)
                .Take(3)
                .Select(g => new GameCardViewModel
                {
                    Id = g.GameId,
                    Title = g.Title ?? "",
                    Price = g.Price,
                    CoverUrl = "/games/" + (g.CoverImage ?? ""),
                    VideoUrl = g.Videos.FirstOrDefault() != null ? g.Videos.FirstOrDefault().Url : null,
                    ScreenshotUrls = g.Screenshots.Select(s => s.ImageUrl).ToList()
                })
                .ToListAsync();

            var randomGames = await _context.Games
                .Where(g => g.IsActive)
                .OrderBy(x => Guid.NewGuid())
                .Include(g => g.Screenshots)
                .Include(g => g.Videos)
                .Take(5)
                .Select(g => new GameCardViewModel
                {
                    Id = g.GameId,
                    Title = g.Title ?? "",
                    Price = g.Price,
                    CoverUrl = "/games/" + (g.CoverImage ?? ""),
                    VideoUrl = g.Videos.FirstOrDefault() != null ? g.Videos.FirstOrDefault().Url : null,
                    ScreenshotUrls = g.Screenshots.Select(s => s.ImageUrl).ToList()
                })
                .ToListAsync();

            // Öne çıkan ve rastgele oyunları birleştir ve karıştır
            model.SliderGames = featuredGames.Concat(randomGames)
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .ToList();

            // Öne çıkan oyunlar
            model.FeaturedGames = await _context.Games
                .Where(g => g.IsActive && g.IsFeatured)
                .Take(8)
                .Select(g => new GameCardViewModel
                {
                    Id = g.GameId,
                    Title = g.Title ?? "",
                    Price = g.Price,
                    CoverUrl = "/games/" + (g.CoverImage ?? "")
                })
                .ToListAsync();

            // Bundle'ları getir
            model.Bundles = await _context.Bundles
                .Where(b => b.IsActive)
                .Include(b => b.Games)
                .Take(8)
                .Select(b => new BundleViewModel
                {
                    BundleId = b.BundleId,
                    Name = b.Name,
                    Description = b.Description,
                    Price = b.Price,
                    DiscountPercentage = b.DiscountPercentage,
                    CoverImage = b.Games.FirstOrDefault() != null && b.Games.FirstOrDefault().CoverImage != null ? "/games/" + b.Games.FirstOrDefault().CoverImage : "/games/default-cover.jpg",
                    GameCount = b.Games.Count,
                    GameNames = b.Games.Select(g => g.Title).ToList(),
                    GameImages = b.Games.Select(g => !string.IsNullOrEmpty(g.CoverImage) ? "/games/" + g.CoverImage : "").ToList()
                })
                .ToListAsync();

            // Kategorileri getir
            var categories = await _context.Categories
                .Include(c => c.GameCategories)
                .Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description,
                    BackgroundImage = "", // Geçici olarak boş bırakıyoruz
                    GameCount = c.GameCategories.Count
                })
                .ToListAsync();

            // Background image'ları ayrı olarak set ediyoruz
            foreach (var category in categories)
            {
                category.BackgroundImage = GetCategoryBackgroundImage(category.Name);
            }

            model.Categories = categories;

            // Kategorileri ViewBag'e ekle (JavaScript için)
            ViewBag.Categories = categories;

            // Browse Games
            var browseGames = new BrowseGamesViewModel
            {
                AllGames = await GetGamesByCategory(0),
                TopSellers = await GetTopSellers(),
                SelectedCategoryId = 0,
                SelectedTab = "all"
            };

            model.BrowseGames = browseGames;

            return View(model);
        }

        public IActionResult Store()
        {
            SetViewBagValues();
            return View();
        }

        public async Task<IActionResult> Library(int? selectedGame)
        {
            SetViewBagValues();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının satın aldığı oyunları getir
            var userGames = await _context.UserGames
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Developer)
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Publisher)
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Screenshots)
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Videos)
                .Where(ug => ug.UserId.ToString() == userId)
                .OrderBy(ug => ug.Game.Title)
                .Select(ug => new GameCardViewModel
                {
                    Id = ug.Game.GameId,
                    Title = ug.Game.Title ?? "",
                    Price = ug.Game.Price,
                    CoverUrl = "/games/" + (ug.Game.CoverImage ?? ""),
                    VideoUrl = ug.Game.Videos.FirstOrDefault() != null ? ug.Game.Videos.FirstOrDefault().Url : null,
                    ScreenshotUrls = ug.Game.Screenshots.Select(s => s.ImageUrl).ToList(),
                    Developer = ug.Game.Developer != null ? ug.Game.Developer.Name : "",
                    Publisher = ug.Game.Publisher != null ? ug.Game.Publisher.Name : "",
                    ReleaseDate = ug.Game.ReleaseDate != DateTime.MinValue ? ug.Game.ReleaseDate : (DateTime?)null,
                    PurchaseDate = ug.PurchaseDate
                })
                .ToListAsync();

            // Kullanıcının satın aldığı bundle'ları getir
            var userBundles = await _context.UserBundles
                .Include(ub => ub.Bundle)
                    .ThenInclude(b => b.Games)
                .Where(ub => ub.UserId.ToString() == userId)
                .OrderBy(ub => ub.Bundle.Name)
                .Select(ub => new BundleViewModel
                {
                    BundleId = ub.Bundle.BundleId,
                    Name = ub.Bundle.Name,
                    Description = ub.Bundle.Description,
                    Price = ub.Bundle.Price,
                    DiscountPercentage = ub.Bundle.DiscountPercentage,
                    CoverImage = ub.Bundle.Games.FirstOrDefault() != null && ub.Bundle.Games.FirstOrDefault().CoverImage != null ? "/games/" + ub.Bundle.Games.FirstOrDefault().CoverImage : "/games/default-cover.jpg",
                    GameCount = ub.Bundle.Games.Count,
                    GameNames = ub.Bundle.Games.Select(g => g.Title).ToList(),
                    GameImages = ub.Bundle.Games.Select(g => !string.IsNullOrEmpty(g.CoverImage) ? "/games/" + g.CoverImage : "").ToList(),
                    PurchaseDate = ub.PurchaseDate
                })
                .ToListAsync();

            // Kullanıcının satın aldığı DLC'leri getir
            var userDLCs = await _context.UserDLCs
                .Include(ud => ud.DLC)
                    .ThenInclude(d => d.Game)
                .Where(ud => ud.UserId.ToString() == userId)
                .OrderBy(ud => ud.DLC.Name)
                .Select(ud => new DLCViewModel
                {
                    DLCId = ud.DLC.DLCId,
                    Name = ud.DLC.Name,
                    Description = ud.DLC.Description,
                    Price = ud.DLC.Price,
                    ReleaseDate = ud.DLC.ReleaseDate,
                    CoverImage = "/games/" + (ud.DLC.CoverImage ?? "default-cover.jpg"),
                    GameName = ud.DLC.Game.Title,
                    GameId = ud.DLC.GameId,
                    PurchaseDate = ud.PurchaseDate,
                    IsInstalled = ud.IsInstalled
                })
                .ToListAsync();

            // Seçili oyun için achievement'ları getir
            if (selectedGame.HasValue)
            {
                var selectedGameAchievements = await _context.Achievements
                    .Where(a => a.GameId == selectedGame.Value)
                    .ToListAsync();

                var userAchievements = await _context.UserAchievements
                    .Where(ua => ua.UserId.ToString() == userId && ua.Achievement.GameId == selectedGame.Value)
                    .Select(ua => ua.AchievementId)
                    .ToListAsync();

                // Seçilen oyunun DLC'lerini getir
                var gameDLCs = await _context.DLCs
                    .Where(d => d.GameId == selectedGame.Value)
                    .OrderBy(d => d.Name)
                    .Select(d => new DLCViewModel
                    {
                        DLCId = d.DLCId,
                        Name = d.Name,
                        Description = d.Description,
                        Price = d.Price,
                        ReleaseDate = d.ReleaseDate,
                        CoverImage = "/games/" + (d.CoverImage ?? "default-cover.jpg"),
                        GameName = d.Game.Title,
                        GameId = d.GameId,
                        IsOwned = _context.UserDLCs.Any(ud => ud.DLCId == d.DLCId && ud.UserId.ToString() == userId),
                        IsInstalled = _context.UserDLCs.Any(ud => ud.DLCId == d.DLCId && ud.UserId.ToString() == userId && ud.IsInstalled)
                    })
                    .ToListAsync();

                ViewBag.GameAchievements = selectedGameAchievements;
                ViewBag.UserUnlockedAchievements = userAchievements;
                ViewBag.GameDLCs = gameDLCs;
            }

            ViewBag.UserGames = userGames;
            ViewBag.UserBundles = userBundles;
            ViewBag.UserDLCs = userDLCs;
            
            // Seçili oyunu belirle
            if (selectedGame.HasValue)
            {
                ViewBag.SelectedGame = userGames.FirstOrDefault(g => g.Id == selectedGame.Value);
            }
            else
            {
                ViewBag.SelectedGame = userGames.FirstOrDefault();
            }

            return View();
        }

        public IActionResult Friends()
        {
            SetViewBagValues();
            return View();
        }

        public IActionResult Community()
        {
            SetViewBagValues();
            return View();
        }

        public IActionResult Profile()
        {
            SetViewBagValues();
            return View();
        }

        public IActionResult Wallet()
        {
            SetViewBagValues();
            return View();
        }

        public IActionResult TestMenu()
        {
            SetViewBagValues();
            return View();
        }

        // Test için geçici action
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            try
            {
                // Bundle'ları kontrol et
                var bundles = await _context.Bundles
                    .Include(b => b.Games)
                    .ToListAsync();

                var bundleResult = bundles.Select(b => new
                {
                    BundleId = b.BundleId,
                    Name = b.Name,
                    IsActive = b.IsActive,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    GameCount = b.Games.Count,
                    Games = b.Games.Select(g => g.Title).ToList()
                }).ToList();

                var games = await _context.Games
                    .Include(g => g.Screenshots)
                    .Include(g => g.Videos)
                    .Take(3)
                    .ToListAsync();

                var result = games.Select(g => new
                {
                    Title = g.Title,
                    CoverImage = g.CoverImage,
                    CoverUrl = "/games/" + g.CoverImage,
                    VideoCount = g.Videos.Count,
                    VideoUrls = g.Videos.Select(v => v.Url).ToList(),
                    ScreenshotCount = g.Screenshots.Count,
                    Screenshots = g.Screenshots.Select(s => new
                    {
                        ImageUrl = s.ImageUrl,
                        FullUrl = s.ImageUrl
                    }).ToList()
                }).ToList();

                return Json(new { 
                    bundles = bundleResult,
                    games = result 
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // Kategori arka plan resmini belirle
        private string GetCategoryBackgroundImage(string categoryName)
        {
            return categoryName.ToLower() switch
            {
                "spor" or "sports" => "/games/fc24-cover.jpg",
                "aksiyon" or "action" => "/games/cod-mw3-cover.jpg",
                "macera" or "adventure" => "/games/rdr2-cover.jpg",
                "rpg" => "/games/baldurs-gate-3-cover.jpg",
                "strateji" or "strategy" => "/games/starfield-cover.jpg",
                "yarış" or "racing" => "/games/forza5-cover.jpg",
                "savaş" or "war" => "/games/cod-warzone-cover.jpg",
                "hayatta kalma" or "survival" => "/games/ark-cover.jpg",
                "simülasyon" or "simulation" => "/games/microsoft-flight-simulator-cover.jpg",
                "indie" => "/games/hades-cover.jpg",
                "çok oyunculu" or "multiplayer" => "/games/cs2-cover.jpg",
                "tek oyunculu" or "singleplayer" => "/games/cyberpunk-cover.jpg",
                "açık dünya" or "open world" => "/games/gtav-cover.jpg",
                "fps" or "first person" => "/games/valorant-cover.jpg",
                "tps" or "third person" => "/games/apex-legends-cover.jpg",
                "moba" => "/games/dota2-cover.jpg",
                "battle royale" => "/games/pubg-cover.jpg",
                "souls-like" or "souls like" => "/games/elden-ring-cover.jpg",
                "korku" or "horror" => "/games/dbd-cover.jpg",
                "puzzle" => "/games/portal2-cover.jpg",
                "platform" or "platformer" => "/games/minecraft-cover.jpg",
                "sandbox" => "/games/terraria-cover.jpg",
                "mmo" or "massively multiplayer" => "/games/destiny2-cover.jpg",
                "tactical" or "tactical shooter" => "/games/r6-siege-cover.jpg",
                "fighting" or "fighting game" => "/games/street-fighter-6-cover.jpg",
                "arcade" or "arcade game" => "/games/fall-guys-cover.jpg",
                _ => "/screenshots/kategoriarkaplan.webp" // Varsayılan arka plan
            };
        }

        // Kategoriye göre oyunları getir
        private async Task<List<GameCardViewModel>> GetGamesByCategory(int categoryId)
        {
            var query = _context.Games
                .Include(g => g.GameCategories)
                .Where(g => g.IsActive);
            
            if (categoryId > 0)
            {
                query = query.Where(g => g.GameCategories.ToList().Any(gc => gc.CategoryId == categoryId));
            }

            return await query
                .OrderBy(g => g.Title)
                .Select(g => new GameCardViewModel
                {
                    Id = g.GameId,
                    Title = g.Title ?? "",
                    Price = g.Price,
                    CoverUrl = "/games/" + (g.CoverImage ?? ""),
                    ScreenshotUrls = new List<string>()
                })
                .ToListAsync();
        }

        // En çok satan oyunları getir
        private async Task<List<GameCardViewModel>> GetTopSellers()
        {
            // En popüler oyunları manuel olarak belirle
            var popularGameTitles = new[] { 
                "Counter-Strike 2", 
                "PUBG: BATTLEGROUNDS", 
                "Grand Theft Auto V", 
                "VALORANT", 
                "Dota 2", 
                "EA SPORTS FC 24", 
                "Minecraft", 
                "The Witcher 3: Wild Hunt", 
                "Elden Ring", 
                "Red Dead Redemption 2" 
            };

            var games = await _context.Games
                .Where(g => g.IsActive && popularGameTitles.Contains(g.Title))
                .ToListAsync();

            // Client-side sıralama yapıyoruz
            return games
                .OrderBy(g => Array.IndexOf(popularGameTitles, g.Title))
                .Take(10)
                .Select(g => new GameCardViewModel
                {
                    Id = g.GameId,
                    Title = g.Title ?? "",
                    Price = g.Price,
                    CoverUrl = "/games/" + (g.CoverImage ?? ""),
                    ScreenshotUrls = new List<string>()
                })
                .ToList();
        }

        // AJAX: Kategoriye göre oyunları getir
        [HttpPost]
        public async Task<IActionResult> GetGamesByCategoryAjax([FromBody] CategoryFilterRequest request)
        {
            try
            {
                var games = await GetGamesByCategory(request.CategoryId);
                return Json(new { success = true, games = games });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetGamesBySearchAjax([FromBody] SearchRequest request)
        {
            try
            {
                var games = await _context.Games
                    .Where(g => g.IsActive && g.Title.ToLower().Contains(request.Query.ToLower()))
                    .OrderBy(g => g.Title)
                    .Take(20)
                    .Select(g => new GameCardViewModel
                    {
                        Id = g.GameId,
                        Title = g.Title ?? "",
                        Price = g.Price,
                        CoverUrl = "/games/" + (g.CoverImage ?? ""),
                        ScreenshotUrls = new List<string>()
                    })
                    .ToListAsync();

                return Json(new { success = true, games = games });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoadBalance([FromBody] LoadBalanceRequest request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                {
                    return Json(new { success = false, error = "Kullanıcı bulunamadı" });
                }

                // Bakiyeyi güncelle
                user.WalletBalance += request.Amount + request.Bonus;
                await _context.SaveChangesAsync();

                return Json(new { success = true, newBalance = user.WalletBalance });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SearchUsers([FromBody] SearchRequest request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Kullanıcı adına göre arama yap
                var users = await _context.Users
                    .Where(u => u.Username.Contains(request.Query) && u.UserId != currentId)
                    .Select(u => new
                    {
                        userId = u.UserId,
                        username = u.Username,
                        displayName = u.DisplayName ?? u.Username,
                        isOnline = u.LastLoginDate > DateTime.Now.AddMinutes(-5) // Son 5 dakikada giriş yapmışsa çevrimiçi
                    })
                    .Take(10)
                    .ToListAsync();

                return Json(new { success = true, users = users });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Arama sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestRequest request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Kendine arkadaşlık isteği gönderemez
                if (currentId == request.TargetUserId)
                {
                    return Json(new { success = false, error = "Kendinize arkadaşlık isteği gönderemezsiniz" });
                }

                // Hedef kullanıcı var mı kontrol et
                var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.TargetUserId);
                if (targetUser == null)
                {
                    return Json(new { success = false, error = "Kullanıcı bulunamadı" });
                }

                // Zaten arkadaşlık isteği var mı kontrol et
                var existingRequest = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        (f.UserId == currentId && f.FriendId == request.TargetUserId) ||
                        (f.UserId == request.TargetUserId && f.FriendId == currentId));

                if (existingRequest != null)
                {
                    if (existingRequest.Status == FriendshipStatus.Pending)
                    {
                        return Json(new { success = false, error = "Zaten arkadaşlık isteği gönderilmiş" });
                    }
                    else if (existingRequest.Status == FriendshipStatus.Accepted)
                    {
                        return Json(new { success = false, error = "Zaten arkadaşsınız" });
                    }
                }

                // Yeni arkadaşlık isteği oluştur
                var friendship = new Friendship
                {
                    UserId = currentId,
                    FriendId = request.TargetUserId,
                    Status = FriendshipStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync();

                // Alıcıya bildirim gönder
                var sender = await _context.Users.FirstOrDefaultAsync(u => u.UserId == currentId);
                var notification = new Notification
                {
                    UserId = request.TargetUserId,
                    Title = "Arkadaşlık İsteği",
                    Message = $"{sender.DisplayName ?? sender.Username} size arkadaşlık isteği gönderdi",
                    Type = "friend_request",
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                    RelatedId = friendship.Id
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Arkadaşlık isteği gönderildi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "İstek gönderilirken hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetFriendRequests()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Bana gelen arkadaşlık isteklerini getir
                var friendRequests = await _context.Friendships
                    .Where(f => f.FriendId == currentId && f.Status == FriendshipStatus.Pending)
                    .Include(f => f.User)
                    .Select(f => new
                    {
                        requestId = f.Id,
                        userId = f.UserId,
                        username = f.User.Username,
                        displayName = f.User.DisplayName ?? f.User.Username,
                        requestDate = f.CreatedAt
                    })
                    .ToListAsync();

                return Json(new { success = true, requests = friendRequests });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "İstekler getirilirken hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RespondToFriendRequest([FromBody] FriendRequestResponse request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Arkadaşlık isteğini bul
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => f.Id == request.RequestId && f.FriendId == currentId && f.Status == FriendshipStatus.Pending);

                if (friendship == null)
                {
                    return Json(new { success = false, error = "Arkadaşlık isteği bulunamadı" });
                }

                if (request.Accept)
                {
                    // İsteği kabul et
                    friendship.Status = FriendshipStatus.Accepted;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Arkadaşlık isteği kabul edildi" });
                }
                else
                {
                    // İsteği reddet
                    friendship.Status = FriendshipStatus.Rejected;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Arkadaşlık isteği reddedildi" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "İşlem sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetFriendsList()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Kabul edilmiş arkadaşlıkları getir
                var friendships = await _context.Friendships
                    .Where(f => f.Status == FriendshipStatus.Accepted && 
                               (f.UserId == currentId || f.FriendId == currentId))
                    .Include(f => f.User)
                    .Include(f => f.Friend)
                    .ToListAsync();

                var friends = new List<dynamic>();

                foreach (var friendship in friendships)
                {
                    User friendUser;
                    if (friendship.UserId == currentId)
                    {
                        friendUser = friendship.Friend;
                    }
                    else
                    {
                        friendUser = friendship.User;
                    }

                    friends.Add(new
                    {
                        userId = friendUser.UserId,
                        username = friendUser.Username,
                        displayName = friendUser.DisplayName ?? friendUser.Username,
                        isOnline = friendUser.LastLoginDate > DateTime.Now.AddMinutes(-5), // Son 5 dakikada giriş yapmışsa çevrimiçi
                        lastSeen = friendUser.LastLoginDate
                    });
                }

                // Çevrimiçi ve çevrimdışı arkadaşları ayır
                var onlineFriends = friends.Where(f => f.isOnline).ToList();
                var offlineFriends = friends.Where(f => !f.isOnline).ToList();

                return Json(new { 
                    success = true, 
                    onlineFriends = onlineFriends,
                    offlineFriends = offlineFriends,
                    totalFriends = friends.Count
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Arkadaş listesi getirilirken hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SearchUsersAjax([FromBody] SearchRequest request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                if (string.IsNullOrEmpty(request.Query) || request.Query.Length < 2)
                {
                    return Json(new { success = false, error = "En az 2 karakter girin" });
                }

                var searchQuery = request.Query.ToLower();

                // Kullanıcıları ara (kendisi hariç) - case-insensitive
                var users = await _context.Users
                    .Where(u => u.UserId != currentId && 
                               (u.Username.ToLower().Contains(searchQuery) || 
                                (u.DisplayName != null && u.DisplayName.ToLower().Contains(searchQuery))))
                    .Take(10)
                    .ToListAsync();

                // Zaten arkadaş olan veya istek gönderilmiş kullanıcıları filtrele
                var existingFriendships = await _context.Friendships
                    .Where(f => (f.UserId == currentId || f.FriendId == currentId))
                    .ToListAsync();

                var excludedUserIds = existingFriendships
                    .Select(f => f.UserId == currentId ? f.FriendId : f.UserId)
                    .ToHashSet();

                var filteredUsers = users.Where(u => !excludedUserIds.Contains(u.UserId)).ToList();

                var result = filteredUsers.Select(u => new
                {
                    userId = u.UserId,
                    username = u.Username,
                    displayName = u.DisplayName ?? u.Username,
                    isOnline = u.LastLoginDate > DateTime.Now.AddMinutes(-5)
                }).ToList();

                return Json(new { success = true, users = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Arama sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendRequest request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Arkadaşlık kaydını bul
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => f.Status == FriendshipStatus.Accepted && 
                                             ((f.UserId == currentId && f.FriendId == request.FriendId) ||
                                              (f.UserId == request.FriendId && f.FriendId == currentId)));

                if (friendship == null)
                {
                    return Json(new { success = false, error = "Arkadaşlık kaydı bulunamadı" });
                }

                // Arkadaşlığı kaldır
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Arkadaşlık kaldırıldı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Arkadaşlık kaldırılırken hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProfileSettings()
        {
            SetViewBagValues();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new ProfileSettingsViewModel
            {
                Username = user.Username,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                BirthDate = user.BirthDate,
                ProfilePicture = user.ProfilePicture
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileSettings(ProfileSettingsViewModel model)
        {
            SetViewBagValues();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;
            user.BirthDate = model.BirthDate;
            // Profil fotoğrafı yükleme
            if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
            {
                var fileName = $"avatar_{user.UserId}_{DateTime.Now.Ticks}{System.IO.Path.GetExtension(model.ProfilePictureFile.FileName)}";
                var filePath = Path.Combine("wwwroot/images/avatars", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePictureFile.CopyToAsync(stream);
                }
                user.ProfilePicture = "/images/avatars/" + fileName;
            }
            await _context.SaveChangesAsync();
            
            // ViewBag'i güncelle
            ViewBag.UserProfilePicture = user.ProfilePicture;
            
            ViewBag.Success = "Profil ayarları başarıyla güncellendi.";
            model.ProfilePicture = user.ProfilePicture;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetGameDetails([FromBody] GameDetailsRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Kullanıcı girişi gerekli" });
            }

            var game = await _context.UserGames
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Developer)
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Publisher)
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Screenshots)
                .Include(ug => ug.Game)
                    .ThenInclude(g => g.Videos)
                .Where(ug => ug.UserId.ToString() == userId && ug.Game.GameId == request.GameId)
                .Select(ug => new GameCardViewModel
                {
                    Id = ug.Game.GameId,
                    Title = ug.Game.Title ?? "",
                    Price = ug.Game.Price,
                    CoverUrl = "/games/" + (ug.Game.CoverImage ?? ""),
                    VideoUrl = ug.Game.Videos.FirstOrDefault() != null ? ug.Game.Videos.FirstOrDefault().Url : null,
                    ScreenshotUrls = ug.Game.Screenshots.Select(s => s.ImageUrl).ToList(),
                    Developer = ug.Game.Developer != null ? ug.Game.Developer.Name : "",
                    Publisher = ug.Game.Publisher != null ? ug.Game.Publisher.Name : "",
                    ReleaseDate = ug.Game.ReleaseDate != DateTime.MinValue ? ug.Game.ReleaseDate : (DateTime?)null,
                    PurchaseDate = ug.PurchaseDate
                })
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return Json(new { success = false, message = "Oyun bulunamadı" });
            }

            return Json(new { success = true, game = game });
        }

        [HttpGet]
        public async Task<IActionResult> AddReview(int gameId)
        {
            SetViewBagValues();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının bu oyunu satın alıp almadığını kontrol et
            var userGame = await _context.UserGames
                .Include(ug => ug.Game)
                .FirstOrDefaultAsync(ug => ug.UserId.ToString() == userId && ug.Game.GameId == gameId);

            if (userGame == null)
            {
                return RedirectToAction("Library");
            }

            // Kullanıcının bu oyun için daha önce inceleme yazıp yazmadığını kontrol et
            var existingReview = await _context.GameReviews
                .FirstOrDefaultAsync(r => r.UserId.ToString() == userId && r.GameId == gameId);

            if (existingReview != null)
            {
                // Kullanıcı zaten inceleme yazmış, düzenleme sayfasına yönlendir
                return RedirectToAction("EditReview", new { reviewId = existingReview.ReviewId });
            }

            ViewBag.Game = userGame.Game;
            ViewBag.GameId = gameId;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int gameId, [FromForm] string content, [FromForm] int rating, [FromForm] bool isRecommended)
        {
            SetViewBagValues();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının bu oyunu satın alıp almadığını kontrol et
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userIdInt && ug.Game.GameId == gameId);

            if (userGame == null)
            {
                return RedirectToAction("Library");
            }

            // Kullanıcının bu oyun için daha önce inceleme yazıp yazmadığını kontrol et
            var existingReview = await _context.GameReviews
                .FirstOrDefaultAsync(r => r.UserId == userIdInt && r.GameId == gameId);

            if (existingReview != null)
            {
                // Kullanıcı zaten inceleme yazmış, düzenleme sayfasına yönlendir
                return RedirectToAction("EditReview", new { reviewId = existingReview.ReviewId });
            }

            // Yeni inceleme oluştur
            var review = new GameReview
            {
                UserId = userIdInt,
                GameId = gameId,
                Content = content,
                Rating = rating,
                IsRecommended = isRecommended,
                ReviewDate = DateTime.Now,
                IsVerifiedPurchase = true, // Kullanıcı oyunu satın almış
                PlayTimeAtReview = TimeSpan.Zero, // Şimdilik 0 olarak ayarla
                HelpfulCount = 0,
                NotHelpfulCount = 0,
                IsEdited = false
            };

            _context.GameReviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "İncelemeniz başarıyla eklendi!";
            return RedirectToAction("Details", "Game", new { id = gameId });
        }

        [HttpGet]
        public async Task<IActionResult> EditReview(int reviewId)
        {
            SetViewBagValues();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return RedirectToAction("Login", "Account");
            }

            // İncelemeyi getir
            var review = await _context.GameReviews
                .Include(r => r.Game)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userIdInt);

            if (review == null)
            {
                return RedirectToAction("Library");
            }

            ViewBag.Review = review;
            ViewBag.Game = review.Game;
            ViewBag.GameId = review.GameId;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int reviewId, [FromForm] string content, [FromForm] int rating, [FromForm] bool isRecommended)
        {
            SetViewBagValues();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return RedirectToAction("Login", "Account");
            }

            // İncelemeyi getir
            var review = await _context.GameReviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userIdInt);

            if (review == null)
            {
                return RedirectToAction("Library");
            }

            // İncelemeyi güncelle
            review.Content = content;
            review.Rating = rating;
            review.IsRecommended = isRecommended;
            review.IsEdited = true;
            review.LastEditDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "İncelemeniz başarıyla güncellendi!";
            return RedirectToAction("Details", "Game", new { id = review.GameId });
        }

        [HttpPost]
        public async Task<IActionResult> UnlockAchievement([FromBody] UnlockAchievementRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gerekli" });
                }

                if (!int.TryParse(userId, out int userIdInt))
                {
                    return Json(new { success = false, message = "Geçersiz kullanıcı" });
                }

                // Kullanıcının bu oyunu satın alıp almadığını kontrol et
                var userGame = await _context.UserGames
                    .FirstOrDefaultAsync(ug => ug.UserId == userIdInt && ug.GameId == request.GameId);

                if (userGame == null)
                {
                    return Json(new { success = false, message = "Bu oyunu satın almamışsınız" });
                }

                // Achievement'ı kontrol et
                var achievement = await _context.Achievements
                    .FirstOrDefaultAsync(a => a.AchievementId == request.AchievementId && a.GameId == request.GameId);

                if (achievement == null)
                {
                    return Json(new { success = false, message = "Achievement bulunamadı" });
                }

                // Kullanıcının bu achievement'ı daha önce açıp açmadığını kontrol et
                var existingUserAchievement = await _context.UserAchievements
                    .FirstOrDefaultAsync(ua => ua.UserId == userIdInt && ua.AchievementId == request.AchievementId);

                if (existingUserAchievement != null)
                {
                    return Json(new { success = false, message = "Bu achievement zaten açılmış" });
                }

                // Achievement'ı aç
                var userAchievement = new UserAchievement
                {
                    UserId = userIdInt,
                    AchievementId = request.AchievementId,
                    UnlockDate = DateTime.Now
                };

                _context.UserAchievements.Add(userAchievement);
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = $"Achievement açıldı: {achievement.Name}",
                    achievementName = achievement.Name,
                    achievementIcon = achievement.Icon
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gerekli" });
                }

                if (!int.TryParse(userId, out int userIdInt))
                {
                    return Json(new { success = false, message = "Geçersiz kullanıcı" });
                }

                // Kullanıcının bu oyunu satın alıp almadığını kontrol et
                var userGame = await _context.UserGames
                    .FirstOrDefaultAsync(ug => ug.UserId == userIdInt && ug.GameId == request.GameId);

                if (userGame == null)
                {
                    return Json(new { success = false, message = "Bu oyunu satın almamışsınız" });
                }

                // Oyunu başlatma achievement'ını kontrol et ve aç
                var startGameAchievement = await _context.Achievements
                    .FirstOrDefaultAsync(a => a.GameId == request.GameId && a.Name == "İlk Adım");

                if (startGameAchievement != null)
                {
                    var existingUserAchievement = await _context.UserAchievements
                        .FirstOrDefaultAsync(ua => ua.UserId == userIdInt && ua.AchievementId == startGameAchievement.AchievementId);

                    if (existingUserAchievement == null)
                    {
                        var userAchievement = new UserAchievement
                        {
                            UserId = userIdInt,
                            AchievementId = startGameAchievement.AchievementId,
                            UnlockDate = DateTime.Now
                        };

                        _context.UserAchievements.Add(userAchievement);
                        await _context.SaveChangesAsync();
                    }
                }

                // Son oynama tarihini güncelle
                userGame.LastPlayed = DateTime.Now;
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = "Oyun başlatıldı!",
                    gameId = request.GameId
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlayTime([FromBody] UpdatePlayTimeRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Kullanıcı girişi gerekli" });
                }

                if (!int.TryParse(userId, out int userIdInt))
                {
                    return Json(new { success = false, message = "Geçersiz kullanıcı" });
                }

                // Kullanıcının bu oyunu satın alıp almadığını kontrol et
                var userGame = await _context.UserGames
                    .FirstOrDefaultAsync(ug => ug.UserId == userIdInt && ug.GameId == request.GameId);

                if (userGame == null)
                {
                    return Json(new { success = false, message = "Bu oyunu satın almamışsınız" });
                }

                // Oynama süresini güncelle
                userGame.PlayTime = TimeSpan.FromSeconds(request.PlayTimeSeconds);
                await _context.SaveChangesAsync();

                // Achievement'ları kontrol et
                var achievements = await _context.Achievements
                    .Where(a => a.GameId == request.GameId)
                    .ToListAsync();

                var unlockedAchievements = new List<string>();

                foreach (var achievement in achievements)
                {
                    var existingUserAchievement = await _context.UserAchievements
                        .FirstOrDefaultAsync(ua => ua.UserId == userIdInt && ua.AchievementId == achievement.AchievementId);

                    if (existingUserAchievement == null)
                    {
                        bool shouldUnlock = false;

                        if (achievement.Name == "Başlangıç" && request.PlayTimeSeconds >= 10)
                        {
                            shouldUnlock = true;
                        }
                        else if (achievement.Name == "Tutkulu Oyuncu" && request.PlayTimeSeconds >= 60)
                        {
                            shouldUnlock = true;
                        }

                        if (shouldUnlock)
                        {
                            var userAchievement = new UserAchievement
                            {
                                UserId = userIdInt,
                                AchievementId = achievement.AchievementId,
                                UnlockDate = DateTime.Now
                            };

                            _context.UserAchievements.Add(userAchievement);
                            unlockedAchievements.Add(achievement.Name);
                        }
                    }
                }

                if (unlockedAchievements.Any())
                {
                    await _context.SaveChangesAsync();
                }

                return Json(new { 
                    success = true, 
                    message = "Oynama süresi güncellendi",
                    unlockedAchievements = unlockedAchievements,
                    totalPlayTime = userGame.PlayTime.TotalSeconds
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        public class GameDetailsRequest
        {
            public int GameId { get; set; }
        }

        public class UnlockAchievementRequest
        {
            public int GameId { get; set; }
            public int AchievementId { get; set; }
        }

        public class StartGameRequest
        {
            public int GameId { get; set; }
        }

        public class UpdatePlayTimeRequest
        {
            public int GameId { get; set; }
            public int PlayTimeSeconds { get; set; }
        }
    }
}
