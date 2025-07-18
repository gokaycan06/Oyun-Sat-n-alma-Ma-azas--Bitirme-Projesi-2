using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }

        [StringLength(50, ErrorMessage = "Görünen ad en fazla 50 karakter olabilir")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Kullanım koşullarını kabul etmelisiniz")]
        public bool AcceptTerms { get; set; }
    }
} 