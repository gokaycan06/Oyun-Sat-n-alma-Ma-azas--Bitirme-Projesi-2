using Oyun_Mağaza.Models;

namespace Oyun_Mağaza.ViewModels
{
    public class ChatViewModel
    {
        public int FriendId { get; set; }
        public string FriendName { get; set; }
        public string FriendUsername { get; set; }
        public string FriendAvatar { get; set; }
        public List<Message> Messages { get; set; }
        public int CurrentUserId { get; set; }
    }
} 