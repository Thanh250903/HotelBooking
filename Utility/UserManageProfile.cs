using HotelApp.Models.Others;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HotelApp.Utility
{
    public class UserManageProfile 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManageProfile(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GetUserPictureAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            var currentImage = await _userManager.GetUserAsync(user);
            if (!string.IsNullOrEmpty(currentImage?.ProfilePicture)) 
            {
                return currentImage.ProfilePicture; 
            }
            return currentImage?.ProfilePicture ?? "img/user_image/default-avatar2.jpg";
        }
    }
}
