using HotelApp.Models.Others;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HotelApp.Utility
{
    public class UserManageProfile 
    {
        //public UserManageProfile(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        //{
        //}
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManageProfile(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        // vì UserManager kh có cách thêm ảnh nên phải tạo thêm dịch vụ và cấu hình ở Profilecontroller
        public async Task<string> GetUserPicture(System.Security.Claims.ClaimsPrincipal user)
        {
            var currentImage = await _userManager.GetUserAsync(user);
            //if (user != null && !string.IsNullOrEmpty(user.ProfilePicture))
            //{
            //    string imgPath = Path.Combine("wwwroot", user.ProfilePicture);
            //    if (File.Exists(imgPath))
            //    {
            //        return "/" + user.ProfilePicture;
            //    }
            //}
            // if user avatar = null the system will display this image.
            return currentImage?.ProfilePicture ?? "img/user_image/default-avatar2.jpg";
        }
    }
}
