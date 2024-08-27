using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models.Others
{
    public class ApplicationUser :IdentityUser<int>
    {
        public new int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int UserPhoneNumber { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserAddress { get; set; }
        [Required]
        public bool UserGender { get; set; } // phân biệt giới tính khách hàng 0 là nam 1 là nữ
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
    }
}
