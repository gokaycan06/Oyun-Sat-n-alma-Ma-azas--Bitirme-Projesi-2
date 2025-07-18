using Microsoft.AspNetCore.Mvc;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Helpers;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                ModelState.AddModelError("Username", "Kullanıcı adı gereklidir");
                return View(model);
            }

            var username = model.Username.ToLower();
            var user = _context.Users.FirstOrDefault(u => 
                u.Username.ToLower() == username || 
                u.Email.ToLower() == username);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }

            // Test için basit şifre kontrolü
            if (!PasswordHasher.VerifyPassword(user.PasswordHash, model.Password))
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Hesabınız askıya alınmış. Lütfen destek ekibiyle iletişime geçin.");
                return View(model);
            }

            // Kullanıcı bilgilerini güncelle
            user.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();

            // Authentication cookie oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("DisplayName", user.DisplayName ?? user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? 
                    DateTimeOffset.UtcNow.AddDays(30) : 
                    DateTimeOffset.UtcNow.AddHours(12)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", user.IsAdmin ? "Admin" : "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model, string AcceptTerms)
        {
            model.AcceptTerms = AcceptTerms?.ToLower() == "on" || AcceptTerms?.ToLower() == "true";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kullanıcı adı kontrolü
            if (_context.Users.Any(u => u.Username.ToLower() == model.Username.ToLower()))
            {
                ModelState.AddModelError("Username", "Bu kullanıcı adı zaten kullanılıyor");
                return View(model);
            }

            // E-posta kontrolü
            if (_context.Users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email.ToLower(),
                PasswordHash = PasswordHasher.HashPassword(model.Password),
                DisplayName = string.IsNullOrEmpty(model.DisplayName) ? model.Username : model.DisplayName,
                RegisterDate = DateTime.Now,
                IsActive = true,
                EmailConfirmed = false, // E-posta doğrulama sistemi eklenecek
                WalletBalance = 0,
                ProfilePicture = "/images/default-avatar.png" // Varsayılan profil resmi
            };

            try
            {
                // Navigation property'leri initialize et
                user.UserGames = new List<UserGame>();
                user.UserDLCs = new List<UserDLC>();
                user.UserBundles = new List<UserBundle>();
                user.WishListItems = new List<WishList>();
                user.Reviews = new List<GameReview>();
                user.Achievements = new List<UserAchievement>();
                user.TradingsSent = new List<Trading>();
                user.TradingsReceived = new List<Trading>();

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // TODO: E-posta doğrulama maili gönder

                TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu. Giriş yapabilirsiniz.";
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                // Hatayı logla
                System.Diagnostics.Debug.WriteLine($"Kayıt hatası: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"İç hata: {ex.InnerException.Message}");
                }

                ModelState.AddModelError("", "Hesap oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
                return View(model);
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "E-posta adresi gereklidir");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                // Güvenlik için kullanıcıya hesap bulunamadığını söylemiyoruz
                TempData["SuccessMessage"] = "Eğer bu e-posta adresi kayıtlıysa, şifre sıfırlama bağlantısı gönderilecektir.";
                return RedirectToAction(nameof(Login));
            }

            // TODO: Şifre sıfırlama maili gönder

            TempData["SuccessMessage"] = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
            return RedirectToAction(nameof(Login));
        }
    }
} 