using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
} 