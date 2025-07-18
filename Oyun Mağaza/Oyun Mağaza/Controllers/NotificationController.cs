using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.Models.DTOs;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                // Okunmamış bildirimleri getir
                var notifications = await _context.Notifications
                    .Where(n => n.UserId == currentId && !n.IsRead)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                var notificationList = notifications.Select(n => new
                {
                    id = n.Id,
                    title = n.Title,
                    message = n.Message,
                    type = n.Type,
                    createdAt = n.CreatedAt.ToString("HH:mm"),
                    relatedId = n.RelatedId
                }).ToList();

                return Json(new { 
                    success = true, 
                    notifications = notificationList,
                    count = notifications.Count
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Bildirimler getirilirken hata oluştu" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkNotificationReadRequest request)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == currentId);

                if (notification == null)
                {
                    return Json(new { success = false, error = "Bildirim bulunamadı" });
                }

                notification.IsRead = true;
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "İşlem sırasında hata oluştu" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Geçersiz kullanıcı" });
                }

                var notifications = await _context.Notifications
                    .Where(n => n.UserId == currentId && !n.IsRead)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "İşlem sırasında hata oluştu" });
            }
        }
    }
} 