using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models.Others
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        public string UserPhoneNumber { get; set; }
        [Required]
        public string UserAddress { get; set; }
        [ValidateNever]
        public bool UserGender { get; set; } // phân biệt giới tính khách hàng 0 là nam 1 là nữ
        [ValidateNever]
        public string Role { get;set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true; // Active status
    }
}
