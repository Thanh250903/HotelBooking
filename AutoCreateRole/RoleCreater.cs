using HotelApp.AutoCreateRole;
using HotelApp.Data;
using HotelApp.Models.Others;
using HotelApp.Utility;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace HotelApp.CreateDB
{
    public class RoleCreater : IAutoCreateRole
    {
        private readonly ApplicationDBContext _context;
        private readonly RoleManager<IdentityRole>  _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleCreater(ApplicationDBContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void CreateRole()
        {
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

            // create Admin default account, that can not change

            var adminEmail = "Admin@gmail.com";
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                PhoneNumber = "0968734627",
                UserAddress = "Da Nang city",
                EmailConfirmed = true,
            };

            // Check admin account exists or not
            var adminExists = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
            if (adminExists == null)
            {
                // Create admin account
                _userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
            }
            var admin = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
            // Add Admin role
            if (admin != null && !_userManager.IsInRoleAsync(admin, Constraintt.Admin).GetAwaiter().GetResult())
            {
                _userManager.AddToRoleAsync(admin, Constraintt.Admin).GetAwaiter().GetResult();
            }
        }
    }
}
