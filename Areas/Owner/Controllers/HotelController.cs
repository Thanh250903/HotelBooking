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

namespace HotelApp.Controllers
{
    [Area("Owner")]
    /*[Authorize(Roles = "Owner")]*/ // test with Admin role, because I still not finish Owner Role
    
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

        public IActionResult Hotellist()
        {
            List<Hotel> hotels = _unitOfWork.HotelRepository.GetAll().ToList();
            return View(hotels);
        }

        [AllowAnonymous]
        [Route("Hotel/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var hotel = _unitOfWork.HotelRepository.GetById(id);

            if (hotel == null)
            {
                return NotFound();
            }

            //take room list 
            var rooms = _unitOfWork.RoomRepository.GetRoomsByHotelId(id).Select(room => new RoomVM
            {
                RoomId = room.RoomId,
                HotelId = room.HotelId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Price = room.Price,
                StatusRoom = room.StatusRooms,
                BedCount = room.BedCount,
                RoomImgUrl = room.RoomImgUrl
            }).ToList();

            // Solve data
            ViewBag.Hotels = hotel;

            return View(rooms); // Match model in View

            // Trả về 
            // 1 - thông tin hotel
            // 2 - List rooms

            // View
            // Trả về 1 trong 2
            // ViewBag

            // Cách trả kết quả ra view
            // Nếu trả hotel bằng cách return và list rooms bằng viewBag
            // => (View) mode là hotel, viewBag là list rooms

            // View => show hotel và list room
            // Model: List room
            // ViewBag: Hotel
        }
        public IActionResult CreateHotel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateHotel(Hotel hotel, IFormFile? imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
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
                if(await _userManager.IsInRoleAsync(user, Constraintt.User) && !await _userManager.IsInRoleAsync(user, Constraintt.Owner))
                {
                    await _userManager.AddToRoleAsync(user, Constraintt.Owner);
                }
                _unitOfWork.HotelRepository.Add(hotel);
                _unitOfWork.Save();
                TempData["success"] = "Hotel created successfully";
                TempData["ShowMessage"] = true;
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult EditHotel(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Hotel? hotel = _unitOfWork.HotelRepository.Get(h => h.HotelId == id);
            if (hotel == null)
            {
                NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        public IActionResult EditHotel(Hotel hotel, IFormFile? imageFile)
        {
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
                return RedirectToAction("Index");
            }
            return View(hotel);
        }
        [HttpGet]
        public IActionResult DeleteHotel(int? id)
        {
            Hotel hotel = new Hotel();
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            hotel = _unitOfWork.HotelRepository.Get(h => h.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        public IActionResult DeleteHotel(Hotel hotel)
        {
            _unitOfWork.HotelRepository.Delete(hotel);
            _unitOfWork.Save();
            TempData["success"] = "Hotel deleted successfully";
            TempData["ShowMessage"] = true;
            return RedirectToAction("Index");
        }
    }
}
