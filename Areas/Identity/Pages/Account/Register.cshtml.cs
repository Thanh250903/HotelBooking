﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using HotelApp.Controllers;
using HotelApp.Data;
using HotelApp.Models.Others;
using HotelApp.Repository.IRepository;
using HotelApp.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace HotelApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly Utility.IEmailSender _emailSender;
        private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> userManager;
        private object value1;
        private SignInManager<ApplicationUser> signInManager;
        private ILogger<HomeController> logger;
        private object roleManager;
        private Utility.IEmailSender emailSender;
        private object value2;
        private object value3;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            Utility.IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            ApplicationDBContext dbContext)

        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name ="Name")]
            public string Name { get; set; }
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [Display(Name = "Gender")]
            public bool Gender {  get; set; }
            [Required]
            [Display(Name = "Your phone number")]
            public string PhoneNumber { get; set; }
            [Required]
            [Display(Name = "Your Address")]
            public string Address { get; set; }
            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; } = " User";
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
          
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (Request.Form["registerType"] == "User")
            {
                returnUrl ??= Url.Content("~/");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Name = Input.Name,
                        EmailConfirmed = true,
                        UserAddress = Input.Address,
                        PhoneNumber = Input.PhoneNumber,
                        UserPhoneNumber = Input.PhoneNumber,
                        Role = "User",
                    };
                    if (!new EmailAddressAttribute().IsValid(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Email is not valid.");
                        return Page();
                    }
                    var checkMailExists = await _userManager.FindByEmailAsync(user.Email);
                    //_dbContext.Users.Any(x => x.Email == user.Email);
                    if (checkMailExists != null && checkMailExists.Role == "User") 
                    {
                        TempData["error"] = "User with this email already exists!";
                        return Page();
                    }
                    var result = await _userManager.CreateAsync((ApplicationUser)user, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Create account success");
                        // Notification
                        if (Input.Role == "User")
                        {
                            //await _userManager.AddToRolesAsync(user, new[] { "User" });
                            await _userManager.AddToRoleAsync(user, Input.Role);
                            TempData["success"] = "Create account successfully";
                            return RedirectToPage("/Account/Login", new { area = "Identity" });
                        }
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync((ApplicationUser)user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                // If register failed
            }

            return Page();
        }

        public async Task<IActionResult> OnPostOwnerAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (Request.Form["registerType"] == "Owner")
            {   
                Input.Role = "Owner";
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    var owner = new ApplicationUser()
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        EmailConfirmed = true,
                        Name = Input.Name,
                        UserAddress = Input.Address,
                        PhoneNumber = Input.PhoneNumber,
                        Role = "Owner",
                    };

                    if (!new EmailAddressAttribute().IsValid(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Email is not valid");
                        return Page();
                    }

                    var checkMailExistsOrNot = await _userManager.FindByEmailAsync(owner.Email);
                    if (checkMailExistsOrNot != null)
                    {
                        TempData["error"] = $"Owner with email already exists: {Input.Email} ";
                        return Page();
                    }

                    var results = await _userManager.CreateAsync(owner, Input.Password);
                    if (results.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(owner, Input.Role);

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(owner);
                        var confirmationLink = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { token, email = owner.Email },
                            protocol: Request.Scheme);
                        var message = $"<p>Please confirm your email by clicking the button below:</p><a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; border: none; border-radius: 15px;'>Confirm Email</a>";

                        await _emailSender.SendEmailAsync(owner.Email, "Email Confirmation", message);

                        TempData["success"] = "Owner account created successfully. Please check your email to confirm your account.";
                        return RedirectToPage("/Account/Login", new { area = "Identity" });
                    }

                    foreach (var error in results.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            // If registration failed
            return Page();
        }




        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>) _userStore;

        }
    }
}
