using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<bool> IsAdmin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            return user?.IsAdmin ?? false;
        }

        public async Task<IActionResult> Index()
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var games = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.GameCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(g => g.SystemRequirements)
                .ToListAsync();

            return View(games);
        }

        public async Task<IActionResult> AddGame()
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Developers = await _context.Developers.ToListAsync();
            ViewBag.Publishers = await _context.Publishers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGame(Game game, List<int> selectedCategories, SystemRequirement minReqs, SystemRequirement recReqs)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                game.IsActive = true;

                // Minimum sistem gereksinimleri
                minReqs.Type = "Minimum";
                game.SystemRequirements.Add(minReqs);

                // Önerilen sistem gereksinimleri
                recReqs.Type = "Recommended";
                game.SystemRequirements.Add(recReqs);

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                foreach (var categoryId in selectedCategories)
                {
                    _context.GameCategories.Add(new GameCategory
                    {
                        GameId = game.GameId,
                        CategoryId = categoryId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Developers = await _context.Developers.ToListAsync();
            ViewBag.Publishers = await _context.Publishers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(game);
        }

        public async Task<IActionResult> EditGame(int id)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var game = await _context.Games
                .Include(g => g.GameCategories)
                .Include(g => g.SystemRequirements)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null)
            {
                return NotFound();
            }

            ViewBag.Developers = await _context.Developers.ToListAsync();
            ViewBag.Publishers = await _context.Publishers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategories = game.GameCategories.Select(gc => gc.CategoryId).ToList();
            ViewBag.MinimumRequirements = game.SystemRequirements.FirstOrDefault(sr => sr.Type == "Minimum");
            ViewBag.RecommendedRequirements = game.SystemRequirements.FirstOrDefault(sr => sr.Type == "Recommended");

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(Game game, List<int> selectedCategories, SystemRequirement minReqs, SystemRequirement recReqs)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingGame = await _context.Games
                        .Include(g => g.GameCategories)
                        .Include(g => g.SystemRequirements)
                        .FirstOrDefaultAsync(g => g.GameId == game.GameId);

                    if (existingGame == null)
                    {
                        return NotFound();
                    }

                    // Update game properties
                    existingGame.Title = game.Title;
                    existingGame.Description = game.Description;
                    existingGame.CoverImage = game.CoverImage;
                    existingGame.Price = game.Price;
                    existingGame.ReleaseDate = game.ReleaseDate;
                    existingGame.IsActive = game.IsActive;
                    existingGame.DeveloperId = game.DeveloperId;
                    existingGame.PublisherId = game.PublisherId;

                    // Update system requirements
                    var existingMinReqs = existingGame.SystemRequirements.FirstOrDefault(sr => sr.Type == "Minimum");
                    var existingRecReqs = existingGame.SystemRequirements.FirstOrDefault(sr => sr.Type == "Recommended");

                    if (existingMinReqs != null)
                    {
                        existingMinReqs.OS = minReqs.OS;
                        existingMinReqs.Processor = minReqs.Processor;
                        existingMinReqs.Memory = minReqs.Memory;
                        existingMinReqs.Graphics = minReqs.Graphics;
                        existingMinReqs.Storage = minReqs.Storage;
                        existingMinReqs.AdditionalNotes = minReqs.AdditionalNotes;
                    }
                    else
                    {
                        minReqs.Type = "Minimum";
                        existingGame.SystemRequirements.Add(minReqs);
                    }

                    if (existingRecReqs != null)
                    {
                        existingRecReqs.OS = recReqs.OS;
                        existingRecReqs.Processor = recReqs.Processor;
                        existingRecReqs.Memory = recReqs.Memory;
                        existingRecReqs.Graphics = recReqs.Graphics;
                        existingRecReqs.Storage = recReqs.Storage;
                        existingRecReqs.AdditionalNotes = recReqs.AdditionalNotes;
                    }
                    else
                    {
                        recReqs.Type = "Recommended";
                        existingGame.SystemRequirements.Add(recReqs);
                    }

                    // Update categories
                    _context.GameCategories.RemoveRange(existingGame.GameCategories);
                    foreach (var categoryId in selectedCategories)
                    {
                        _context.GameCategories.Add(new GameCategory
                        {
                            GameId = game.GameId,
                            CategoryId = categoryId
                        });
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Developers = await _context.Developers.ToListAsync();
            ViewBag.Publishers = await _context.Publishers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategories = selectedCategories;

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var game = await _context.Games
                .Include(g => g.SystemRequirements)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game != null)
            {
                _context.SystemRequirements.RemoveRange(game.SystemRequirements);
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
} 