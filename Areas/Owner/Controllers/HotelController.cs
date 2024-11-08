using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using HotelApp.Repository.IRepository;
using HotelApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HotelApp.Controllers
{
    [Area("Owner")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository HotelRepository;
        private ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager <ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HotelController(IHotelRepository hotelRepository, ApplicationDBContext dbContext, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            HotelRepository = hotelRepository;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task< IActionResult> Index()
        {
            var hotels = await _unitOfWork.HotelRepository.GetAllHotelAsync();
            return View(hotels);
        }
        public async Task <IActionResult> Hotellist()
        {
            var ownerId = _userManager.GetUserId(User);
            var hotels = await _unitOfWork.HotelRepository.GetHotelByOwnerId(ownerId);
            return View(hotels);
        }

        [AllowAnonymous]
        [Route("Hotel/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var hotelList = _unitOfWork.HotelRepository.GetHotelByIdAsync(id);
            var hotel = await hotelList;
            if (hotel == null)
            {
                return NotFound();
            }

            var ownerId = _userManager.GetUserId(User);
            var isUser = User.IsInRole("User");
            var isOwner = hotel.OwnerId == ownerId;// checking this Owner this hotel or not
            var isNotOwner = !isOwner && User.IsInRole("Owner");

            var rooms = await _unitOfWork.RoomRepository.GetRoomsByHotelIdAsync(id);
            var roomVM = rooms.Select(room => new RoomVM
            {
                RoomId = room.RoomId,
                HotelId = room.HotelId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Price = room.Price,
                StatusRoom = room.StatusRooms,
                BedCount = room.BedCount,
                RoomImgUrl = room.RoomImgUrl,
                IsOwner = isOwner,
            }).ToList();   //take room list 

            // Solve data
            var hotels = new List<Hotel> { hotel };
            ViewBag.Hotels = hotels;
            ViewBag.IsUser = isUser;
            ViewBag.IsOwner = isOwner;
            ViewBag.IsNotOwner = isNotOwner;

            return View(roomVM);
        }
        [HttpGet]
        public IActionResult CreateHotel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateHotel(Hotel hotel, IFormFile? imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            var ownerId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                // Upload image
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Image path
                    var wwwRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(wwwRootPath, fileName);

                    if (!Directory.Exists(wwwRootPath))
                    {
                        Directory.CreateDirectory(wwwRootPath);
                    }
                    else
                    {
                        Console.WriteLine("Image not found", fileName);
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    hotel.ImageUrl = $"/img/" + fileName;
                }

                hotel.OwnerId = ownerId;
                if(await _userManager.IsInRoleAsync(user, Constraintt.User) && !await _userManager.IsInRoleAsync(user, Constraintt.Owner))
                {
                    await _userManager.AddToRoleAsync(user, Constraintt.Owner);
                }
                _unitOfWork.HotelRepository.Add(hotel);
                _unitOfWork.Save();
                TempData["success"] = "Hotel created successfully";
                TempData["ShowMessage"] = true;
                return RedirectToAction("HotelList");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditHotel(int? id)
        {
            
            if (id == null || id == 0)
            {
               return NotFound();
            }
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(id.Value, ownerId))
            {
                return Unauthorized();
            }
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id.Value);
            if (hotel == null)
            {
                NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> EditHotel(Hotel hotel, IFormFile? imageFile, int hotelId)
        {
            if(hotelId != hotel.HotelId)
            {
                return NotFound();
            }
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(hotelId, ownerId))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string imagePath = Path.Combine(wwwRootPath, "img");

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    hotel.ImageUrl = "/img/" + fileName;
                }
                _unitOfWork.HotelRepository.Update(hotel);
                _unitOfWork.Save();
                TempData["success"] = "Hotel Updated successfully";
                return RedirectToAction("HotelList");
            }
            return View(hotel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteHotel(int? id)
        {
            Hotel hotel = new Hotel();
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id.Value);
            if (hotel == null)
            {
                return NotFound();  
            }
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(id.Value, ownerId))
            {
                return Unauthorized();
            }
            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHotel(Hotel hotel)
        {
            var ownerId = _userManager.GetUserId(User);
            if(!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(hotel.HotelId, ownerId)) 
            {
                return Unauthorized();
            }

            _unitOfWork.HotelRepository.Delete(hotel);
            _unitOfWork.Save();
            TempData["success"] = "Hotel deleted successfully";
            TempData["ShowMessage"] = true;
            return RedirectToAction("HotelList");
        }
    }
}
