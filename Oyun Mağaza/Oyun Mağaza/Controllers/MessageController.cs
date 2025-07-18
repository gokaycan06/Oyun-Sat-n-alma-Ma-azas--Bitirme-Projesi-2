using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Oyun_Mağaza.Data;
using Oyun_Mağaza.Models;
using Oyun_Mağaza.ViewModels;
using Oyun_Mağaza.Models.DTOs;
using System.Security.Claims;

namespace Oyun_Mağaza.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
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

        public async Task<IActionResult> Chat(int friendId)
        {
            try
            {
                SetViewBagValues();
                
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return RedirectToAction("Login", "Account");
                }

                // Arkadaşlık kontrolü
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => f.Status == FriendshipStatus.Accepted && 
                                             ((f.UserId == currentId && f.FriendId == friendId) ||
                                              (f.UserId == friendId && f.FriendId == currentId)));

                if (friendship == null)
                {
                    return RedirectToAction("Friends", "Home");
                }

                // Arkadaş bilgilerini al
                var friend = await _context.Users.FirstOrDefaultAsync(u => u.UserId == friendId);
                if (friend == null)
                {
                    return RedirectToAction("Friends", "Home");
                }

                // Önceki mesajları al (son 50 mesaj)
                var messages = await _context.Messages
                    .Where(m => (m.SenderId == currentId && m.ReceiverId == friendId) ||
                               (m.SenderId == friendId && m.ReceiverId == currentId))
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(50)
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();

                var viewModel = new ChatViewModel
                {
                    FriendId = friendId,
                    FriendName = friend.DisplayName ?? friend.Username,
                    FriendUsername = friend.Username,
                    FriendAvatar = !string.IsNullOrEmpty(friend.ProfilePicture) ? friend.ProfilePicture : "/images/avatars/default-avatar.jpg",
                    Messages = messages,
                    CurrentUserId = currentId
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Friends", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                // Arkadaşlık kontrolü
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => f.Status == FriendshipStatus.Accepted && 
                                             ((f.UserId == currentId && f.FriendId == request.ReceiverId) ||
                                              (f.UserId == request.ReceiverId && f.FriendId == currentId)));

                if (friendship == null)
                {
                    return Json(new { success = false, error = "Bu kullanıcıya mesaj gönderemezsiniz" });
                }

                if (string.IsNullOrEmpty(request.Content?.Trim()))
                {
                    return Json(new { success = false, error = "Mesaj boş olamaz" });
                }

                var message = new Message
                {
                    SenderId = currentId,
                    ReceiverId = request.ReceiverId,
                    Content = request.Content.Trim(),
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // Alıcıya bildirim gönder
                var sender = await _context.Users.FirstOrDefaultAsync(u => u.UserId == currentId);
                var notification = new Notification
                {
                    UserId = request.ReceiverId,
                    Title = "Yeni Mesaj",
                    Message = $"{sender.DisplayName ?? sender.Username} size mesaj gönderdi",
                    Type = "message",
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                    RelatedId = message.Id
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    messageId = message.Id,
                    createdAt = message.CreatedAt.ToString("HH:mm")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Mesaj gönderilirken hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetNewMessages([FromBody] GetMessagesRequest request)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || !int.TryParse(currentUserId, out int currentId))
                {
                    return Json(new { success = false, error = "Kullanıcı girişi yapılmamış" });
                }

                var messages = await _context.Messages
                    .Where(m => (m.SenderId == currentId && m.ReceiverId == request.FriendId) ||
                               (m.SenderId == request.FriendId && m.ReceiverId == currentId))
                    .Where(m => m.Id > request.LastMessageId)
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();

                var messageList = messages.Select(m => new
                {
                    id = m.Id,
                    content = m.Content,
                    senderId = m.SenderId,
                    isOwn = m.SenderId == currentId,
                    createdAt = m.CreatedAt.ToString("HH:mm")
                }).ToList();

                return Json(new { success = true, messages = messageList });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Mesajlar getirilirken hata oluştu" });
            }
        }
    }
} 