using HotelApp.Data;
using HotelApp.Models;
using HotelApp.Repository.IRepository;
using HotelApp.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HotelApp.Repository;
using HotelApp.Utility;
using Microsoft.AspNetCore.Identity;
using HotelApp.Models.Others;
using HotelApp.Areas.Identity.Pages.Account;
using HotelApp.Constants;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace HotelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelRepository HotelRepository;
        private ApplicationDBContext _dbContext;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RegisterModel _registerModel;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(IHotelRepository hotelRepository, ApplicationDBContext dBContext, IEmailSender emailSender, RegisterModel registerModel, ILogger<RegisterModel> logger, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            HotelRepository = hotelRepository;
            _dbContext = dBContext;
            _emailSender = emailSender;
            _registerModel = registerModel;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Hotel> hotels = _dbContext.Hotels.ToList();
            return View(hotels);
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult CreateHotel()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ManageUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendConfirmationEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendConfirmationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Email address cannot be null or empty");
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && user.Role == "Owner")
            {
                TempData["error"] = $"This email address is already registered with Owner role. PLease try other email {email}.";
                return RedirectToAction("SendConfirmationEmail", "Home");
            }
            // tạo tk user tạm thời
            var newUser = new ApplicationUser()
            {
                UserName = email,
                Email = email,
                Role = "Owner",
                Name = "Default Name", // Giá trị mặc định cho Name
                UserAddress = "Default Address",
                UserPhoneNumber = "00000000",
                UserGender = true,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,    
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                TempData["error"] = "Error when try creating new account";
                return RedirectToAction("SendConfirmationEmail", "Home");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            if (token == null)  
            {
                TempData["error"] = "Error generating email confirmation token.";
                return View(); 
            }

            var confirmationLink = Url.Action("ConfirmEmail", "Home", new { email = newUser.Email, token }, Request.Scheme);
            string subject = "Email Confirmation";
            var message = $"<p>Please confirm your email by clicking the button below:</p><a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; border: none; border-radius: 15px;'>Confirm Email</a>";

            await _emailSender.SendEmailAsync(email, subject, message);
            ViewBag.Message = "The system has sent a confirmation email to your email address. Please check it.";
            return View();
        } 
        
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Email and token are required");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Invalid user :{email}";
                return RedirectToAction("SendConfirmationEmail", "Home");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["success"] = "Your email has been confirmed successfully!";
                return RedirectToAction("RegisterDetail", new { email, token }); // Chuyển đến trang đăng ký Owner
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult RegisterDetail(string token, string email)
        {
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = email,
                Role = "Owner"
            };
            ViewData["Token"] = token;
            ViewData["Title"] = "Register Detail";
            return View(_registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDetail(RegisterModel.InputModel input, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("token", "The token field is required.");
                return View(_registerModel);
            }
            if (ModelState.IsValid)
            {
                var owner = await _userManager.FindByEmailAsync(input.Email);
                if (owner == null)
                {
                    return View("Error");
                }

                var result = await _userManager.ConfirmEmailAsync(owner, token);
                if (result.Succeeded)
                {
                    owner.UserName = input.Email;
                    owner.Email = input.Email;
                    owner.Name = input.Name;
                    owner.UserAddress = input.Address;
                    owner.PhoneNumber = input.PhoneNumber;
                    owner.Role = "Owner";

                    await _userManager.UpdateAsync(owner);
                    await _userManager.AddToRoleAsync(owner,"Owner");

                    TempData["success"] = "Owner account created successfully";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }
                foreach (var checkerror in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, checkerror.Description);
                }
            }
            else
            {
                TempData["erorr"] = "Owner account created fail, try again";
            }
            ViewData["Title"] = "Register Detail";
            return View(_registerModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
