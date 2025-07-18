using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Oyun_Mağaza.ViewModels
{
    public class ProfileSettingsViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "E-posta")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Görünen İsim")]
        public string? DisplayName { get; set; }

        [Display(Name = "Biyografi")]
        public string? Bio { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public string? ProfilePicture { get; set; }

        public IFormFile? ProfilePictureFile { get; set; }
    }
} 