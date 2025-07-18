using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
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
            
            var category = await _context.Categories
                .Include(c => c.GameCategories)
                .ThenInclude(gc => gc.Game)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            var games = category.GameCategories
                .Where(gc => gc.Game.IsActive)
                .Select(gc => gc.Game)
                .ToList();

            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryDescription = category.Description;

            return View(games);
        }
    }
} 