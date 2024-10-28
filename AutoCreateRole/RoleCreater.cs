using HotelApp.AutoCreateRole;
using HotelApp.Data;
using HotelApp.Models.Others;
using HotelApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace HotelApp.CreateDB
{
    public class RoleCreater : IAutoCreateRole
    {
        private readonly ApplicationDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleCreater(ApplicationDBContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task CreateRole()
        {
            //string[] roleNames = { "Admin", "Owner", "Staff", "User" };
            //foreach (var roleName in roleNames)
            //{
            //    // Kiểm tra và tạo role nếu chưa tồn tại
            //    if (!await _roleManager.RoleExistsAsync(roleName))
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole(roleName));
            //    }
            //}
            if (_context.Roles.Any(role => role.Name == Constraintt.Admin || role.Name == Constraintt.Owner ||
                                 role.Name == Constraintt.Staff || role.Name == Constraintt.User))
                return;

            if (_context.Roles.Any(role => role.Name == Constraintt.Admin)) return;
            if (_context.Roles.Any(role => role.Name == Constraintt.Owner)) return;
            if (_context.Roles.Any(role => role.Name == Constraintt.Staff)) return;
            if (_context.Roles.Any(role => role.Name == Constraintt.User)) return;

            _roleManager.CreateAsync(new IdentityRole(Constraintt.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constraintt.Owner)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constraintt.Staff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constraintt.User)).GetAwaiter().GetResult();

            // Tạo tài khoản Admin mặc định nếu chưa tồn tại
            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin@123";

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin",
                    Role = "Admin",
                    UserAddress = "Da Nang",
                    UserPhoneNumber = "0968734627",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
                var admin = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
                // Add Admin role
                if (admin != null && !_userManager.IsInRoleAsync(admin, Constraintt.Admin).GetAwaiter().GetResult())
                {
                    _userManager.AddToRoleAsync(admin, Constraintt.Admin).GetAwaiter().GetResult();
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
    }
}
