using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models.Others
{
    public class ApplicationUser :IdentityUser
    {
        [ValidateNever]
        public string UserPhoneNumber { get; set; }
        [ValidateNever]
        public string UserAddress { get; set; }
        [ValidateNever]
        public bool UserGender { get; set; } // phân biệt giới tính khách hàng 0 là nam 1 là nữ 
        [ValidateNever]
        public string Role { get;set; } // User, Admin, Owner
        [ValidateNever]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ValidateNever]
        public bool IsActive { get; set; } = true; // Active status
    }
}
