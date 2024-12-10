using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using HotelApp.Repository.IRepository;
using HotelApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


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
        [HttpGet]
        [Route("Hotel/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            // Query a hotel by id
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                TempData["error"] = "Hotel not found to display detail";
                return NotFound();
            }

            // Lấy thông tin người dùng hiện tại và kiểm tra quyền sở hữu
            var ownerId = _userManager.GetUserId(User);
            var isUser = User.IsInRole("User");
            var isOwner = hotel.OwnerId == ownerId;
            var isNotOwner = !isOwner && User.IsInRole("Owner");

            // Lấy các đánh giá về khách sạn
            var reviews = await _unitOfWork.HotelReviewRepository.GetReviewsByHotelIdAsync(id);
            var rooms = await _unitOfWork.RoomRepository.GetRoomsByHotelIdAsync(id);
            var hotelDetailsVM = new HotelDetailsVM
            {
                HotelId = hotel.HotelId,
                HotelName = hotel.HotelName,
                Description = hotel.Description,
                City = hotel.City,
                ImageUrl = hotel.ImageUrl,
                IsOwner = isOwner,
                Rooms = rooms.Select(room => new RoomVM
                {
                    RoomId = room.RoomId,
                    HotelId = hotel.HotelId,
                    RoomNumber = room.RoomNumber,
                    RoomType = room.RoomType,
                    Price = room.Price,
                    StatusRoom = room.StatusRooms,
                    BedCount = room.BedCount,
                    RoomImgUrl = room.RoomImgUrl,
                    IsOwner = isOwner,
                }).ToList(),
                HotelReviews = reviews,
                NewHotelReview = new HotelReview
                {
                    HotelId = hotel.HotelId,
                    Comment = ""
                },
                TotalReviews = reviews.Count()

            };
            ViewBag.IsUser = isUser;
            ViewBag.IsOwner = isOwner;
            ViewBag.IsNotOwner = isNotOwner;
            ViewBag.UserManager = _userManager;

            return View(hotelDetailsVM);
        }

        //[AllowAnonymous]
        [HttpPost]
        [Route("/Hotel/CreateReview")]
        public async Task<IActionResult> CreateReview([Bind("HotelId, Comment, Image")] HotelReview hotelReview, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                if(Image != null && Image.Length > 0)
                {
                    var wwwRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var fileName = Path.GetFileName(Image.FileName);
                    var filePath = Path.Combine(wwwRootPath, fileName);

                    try
                    {
                        if (!Directory.Exists(wwwRootPath))
                        {
                            Directory.CreateDirectory(wwwRootPath);
                        }
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }
                        hotelReview.Image = $"/img/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Image upload error: " + ex.Message);
                        return View(hotelReview);
                    }
                }
                else
                {
                    hotelReview.Image = hotelReview.Image;
                }

                // Check the data to leave the comments
                Console.WriteLine($"HotelId: {hotelReview.HotelId}, Comment: {hotelReview.Comment}, Image: {hotelReview.Image}");

                var userId = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : null;
                hotelReview.UserId = userId;
                hotelReview.CreateAt = DateTime.Now;

                _unitOfWork.HotelReviewRepository.Add(hotelReview);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Your comment added successfully";
                return RedirectToAction("Details", new
                {
                    id = hotelReview.HotelId 
                });
            }
            TempData["error"] = "Failed to add your review. Please check your input.";
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(hotelReview.HotelId);
            if (hotel == null)
            {
                TempData["error"] = "Hotel not exits";
                return NotFound();
            }

            var reviews = await _unitOfWork.HotelReviewRepository.GetReviewsByHotelIdAsync(hotelReview.HotelId);
            var rooms = await _unitOfWork.RoomRepository.GetRoomsByHotelIdAsync(hotelReview.HotelId);

            var hotelDetailsVM = new HotelDetailsVM
            {
                HotelId = hotel.HotelId,
                HotelName = hotel.HotelName,
                Description = hotel.Description,
                City = hotel.City,
                ImageUrl = hotel.ImageUrl,
                IsOwner = hotel.OwnerId == _userManager.GetUserId(User),
                Rooms = rooms.Select(room => new RoomVM
                {
                    RoomId = room.RoomId,
                    HotelId = hotel.HotelId,
                    RoomNumber = room.RoomNumber,
                    RoomType = room.RoomType,
                    Price = room.Price,
                    StatusRoom = room.StatusRooms,
                    BedCount = room.BedCount,
                    RoomImgUrl = room.RoomImgUrl,
                    IsOwner = hotel.OwnerId == _userManager.GetUserId(User),
                }).ToList(),
                HotelReviews = reviews,
                NewHotelReview = hotelReview,
            };

            ViewBag.IsUser = User.IsInRole("User");
            ViewBag.IsOwner = hotel.OwnerId == _userManager.GetUserId(User);
            ViewBag.IsNotOwner = !ViewBag.IsOwner && User.IsInRole("Owner");

            return View("Details", hotelDetailsVM);
        }
        [HttpPost]
        [Authorize(Roles = "User, Owner")]
        public async Task<IActionResult> DeleteReview (int reviewId)
        {
            var review = await _unitOfWork.HotelReviewRepository.GetByIdAsync(reviewId);
            if(review == null)
            {
                TempData["error"] = "Couldn't found the review you want to delete";
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var isOwner = review.Hotel.OwnerId == userId;
            var isUser = review.UserId == userId;

            if(!isOwner && isUser)
            {
                TempData["error"] = "You don't have premission to delete this review";
                return Unauthorized();
            }

            _unitOfWork.HotelReviewRepository.Delete(review);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Review deleted successfully";
            return RedirectToAction("Details", new 
            {
                id = review.HotelId 
            });
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
            else
            {
                TempData["error"] = "Create hotel failed, try again";
                TempData["ShowMessage"] = true;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditHotel(int? id)
        {
            
            if (id == null || id == 0)
            {
               TempData["error"] = "Hotel id not exits";
               return NotFound();
            }
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(id.Value, ownerId))
            {
                TempData["error"] = "You don't have premission to edit this hotel";
                return Unauthorized();
            }
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id.Value);
            if (hotel == null)
            {
                TempData["error"] = "Counld't found a hotel to edit";
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
                TempData["error"] = "Hotel Id not found";
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
                TempData["error"] = "Hotel Id not found";
                return NotFound();
            }
            hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id.Value);
            if (hotel == null)
            {
                TempData["error"] = "Hotel not found";
                return NotFound();  
            }
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(id.Value, ownerId))
            {
                TempData["error"] = "You don't have premission to edit this hotel";
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
                TempData["error"] = "You don't have premission to delete this hotel";
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
