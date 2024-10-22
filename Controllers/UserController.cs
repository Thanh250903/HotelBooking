﻿using HotelApp.Data;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HotelApp.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDBContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userList = _dbContext.Users.Where(user => user.Id != claims.Value);
            
            foreach(var user in userList)
            {
                var userTemp = await _userManager.FindByIdAsync(user.Id);
                if (userTemp != null)
                {
                    return NotFound();
                }
                var roleTemp = await _userManager.GetRolesAsync(userTemp);
                user.Role = roleTemp.FirstOrDefault();
            }
            return View(userList);
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var manageUser = _dbContext.Users.Find(id);
            return View(manageUser);
        }

        [HttpPost]
        public IActionResult EditUser(ApplicationUser user)
        {
            if (user == null)
            {
                return NotFound("User not found");
            }
            var manageUser = _dbContext.User.Find(user.Id);
            if (manageUser == null)
            {
                return NotFound("User not found");
            }
            manageUser.Name = user.Name;
            manageUser.PhoneNumber = user.PhoneNumber;
            manageUser.UserAddress = user.UserAddress;
            manageUser.UserGender = user.UserGender;

            _dbContext.Users.Update(manageUser);
            _dbContext.SaveChanges();
            TempData["success"] = "Users edited successfully";
            TempData["ShowMessage"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _dbContext.Users.Find(id);
            var roleTemp = await _userManager.GetRolesAsync(user);
            var role = roleTemp.First();

            return RedirectToAction("EditUser", new { id });
        }
       
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound("User not found");
            }
            await _userManager.DeleteAsync(user);
            TempData["success"] = "User deleted successfully";
            TempData["ShowMessage"] = true;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ResetPasswordVM model = new ResetPasswordVM()
            {
                Email = user.Email,
                Password = user.PasswordHash,
                ConfirmPassword = user.PasswordHash
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var email = Request.Form["email"];
            var password = Request.Form["password"];
            var user = await _userManager.FindByEmailAsync(email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (user == null)
                return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                TempData["success"] = $"Password reset successfully! for (Email:  {user.Email})";
                return RedirectToAction("Index");
            }
            
            // if reset password wrong
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            resetPasswordVM = new ResetPasswordVM()
            {
                Email = user.Email,
                Password = user.PasswordHash,
                ConfirmPassword = user.PasswordHash,
                Token = user.Id
            };
            TempData["error"] = "Something wrong!";
            return View(resetPasswordVM);
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        //Lock account
        [HttpGet]
        public async Task<IActionResult> LockAccount(string id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var lockingUser = _dbContext.Users.Where(user => user.Id == id).First();

            if(lockingUser.Id  == claims.Value)
            {

            }

            if (lockingUser.LockoutEnd != null && lockingUser.LockoutEnd > DateTime.Now)
                lockingUser.LockoutEnd = DateTime.Now;
            else
               lockingUser.LockoutEnd = DateTime.Now.AddYears(1000);
            _dbContext.SaveChanges();
            TempData["success"] = "Locking User successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}