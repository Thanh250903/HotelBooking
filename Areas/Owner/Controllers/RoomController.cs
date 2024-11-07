using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class RoomController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoomRepository _roomRepository;

        public RoomController(ApplicationDBContext dbContext, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IRoomRepository roomRepository, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _roomRepository = roomRepository;
            _userManager = userManager;
        }
        // hiển thị danh sách phòng dựa trên Id của Hotel
        [HttpGet] // lấy data từ Hotel
        [Route("Index")]
        public IActionResult Index(int hotelId)
        {
            List<Room> rooms = _unitOfWork.RoomRepository.GetRoomsByHotelId(hotelId).ToList();
            return View(rooms);
        }
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateRoom(int id)
        {
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(id, ownerId))
            {
                return Unauthorized();
            }

            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound("Cannot find the hotel to create room");
            }

            RoomVM roomVM = new RoomVM
            {
                HotelId = id,
                HotelName = hotel.HotelName
            };
            return View(roomVM);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateRoom(RoomVM roomVM, IFormFile? roomImages)
        {
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(roomVM.HotelId, ownerId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                //Handle pictures room
                if (roomImages != null && roomImages.Length > 0)
                {
                    var wwwRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var fileName = Path.GetFileName(roomImages.FileName);
                    var filePath = Path.Combine(wwwRootPath, fileName);

                    try
                    {
                        if (!Directory.Exists(wwwRootPath))
                        {
                            Directory.CreateDirectory(wwwRootPath);
                        }
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await roomImages.CopyToAsync(fileStream); // Lưu tệp một cách bất đồng bộ (asynchronously)
                        }
                        roomVM.RoomImgUrl = $"/img/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Image upload error: " + ex.Message);
                        return View(roomVM);
                    }
                }

                Room room = new Room
                {
                    HotelId = roomVM.HotelId,
                    RoomNumber = roomVM.RoomNumber,
                    RoomType = roomVM.RoomType,
                    Price = roomVM.Price,
                    StatusRooms = roomVM.StatusRoom,
                    BedCount = roomVM.BedCount,
                    RoomImgUrl = roomVM.RoomImgUrl,
                };

                // check if Hotel Exists 
                var hotelExists = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
                if (hotelExists == null)
                {
                    ModelState.AddModelError("", "Invalid HotelId");
                    return View(roomVM);
                }

                //Check roomnumber same name
                if (!await _unitOfWork.RoomRepository.IsRoomNumberUniqueAsync(roomVM.HotelId, roomVM.RoomNumber))
                {
                    ModelState.AddModelError("", "Room number already exits for this hotel, try again!!!");
                    return View(roomVM);
                }

                //Add room into hotel method
                await _unitOfWork.RoomRepository.AddRoom(room);
                await _unitOfWork.RoomRepository.SaveAsync();
                TempData["success"] = "Add room successfully";// bool
                TempData["ShowMessage"] = true;

                return RedirectToAction("Details", "Hotel", new
                {
                    id = roomVM.HotelId
                });
            }
            else
            {
                TempData["error"] = "Add room failed";
            }
            return View(roomVM);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoom(int id)
        {
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                return NotFound();
            }
            var ownerId = _userManager.GetUserId(User);
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
            if (hotel == null || hotel.OwnerId != ownerId)
            {
                return Unauthorized();
            }
            RoomVM roomVM = new RoomVM
            {
                HotelId = room.HotelId,
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Price = room.Price,
                StatusRoom = room.StatusRooms,
                BedCount = room.BedCount,
                RoomImgUrl = room.RoomImgUrl,
            };
            return View(roomVM);
        }

        [HttpPost]

        public async Task<IActionResult> EditRoom(RoomVM roomVM, IFormFile? roomImages)
        {
            var ownerId = _userManager.GetUserId(User);
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(roomVM.HotelId, ownerId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                // Handle image update
                var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(roomVM.HotelId);
                if (room == null)
                {
                    ModelState.AddModelError("", "Room not found");
                    return View(roomVM);
                }

                if (roomImages != null && roomImages.Length > 0)
                {
                    var wwwRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var fileName = Path.GetFileName(roomImages.FileName);
                    var filePath = Path.Combine(wwwRootPath, fileName);

                    try
                    {
                        if (!Directory.Exists(wwwRootPath))
                        {
                            Directory.CreateDirectory(wwwRootPath);
                        }
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await roomImages.CopyToAsync(fileStream);
                        }
                        roomVM.RoomImgUrl = $"/img/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Image upload error: " + ex.Message);
                        return View(roomVM);
                    }

                }

                //room.RoomId = roomVM.RoomId;
                room.RoomNumber = roomVM.RoomNumber;
                room.RoomType = roomVM.RoomType;
                room.Price = roomVM.Price;
                room.StatusRooms = roomVM.StatusRoom;
                room.BedCount = roomVM.BedCount;
                room.RoomImgUrl = roomVM.RoomImgUrl;

                if (!_unitOfWork.RoomRepository.IsRoomNumberUnique(roomVM.HotelId, roomVM.RoomNumber))
                {
                    ModelState.AddModelError("", "Room number already exists for this hotel.");
                    return View(roomVM);
                }
                if (ModelState.IsValid)
                {
                    _unitOfWork.RoomRepository.Update(room);
                    _unitOfWork.RoomRepository.SaveAsync();
                    TempData["success"] = "Edit room successfully";
                    TempData["ShowMessage"] = true;
                    return RedirectToAction("Details", "Hotel", new
                    {
                        id = roomVM.HotelId
                    });
                }
            }
            return View(roomVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                TempData["error"] = "Room not found. Deletion failed.";
                return RedirectToAction("Index", "Hotel");
            }

            var ownerId = _userManager.GetUserId(User);
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
            if (hotel.OwnerId != ownerId)
            {
                return Unauthorized();
            }

            RoomVM roomVM = new RoomVM
            {
                RoomId = room.RoomId,
                HotelId = room.HotelId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Price = room.Price,
                StatusRoom = room.StatusRooms,
                BedCount = room.BedCount,
                RoomImgUrl = room.RoomImgUrl,
            };
            return View(roomVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(RoomVM roomVM)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(roomVM.RoomId);
            if (room == null)
            {
                TempData["error"] = "Room not found, try again";
                return RedirectToAction("HotelList");
            }

            var owerId = _userManager.GetUserId(User);
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
            if (hotel.OwnerId != owerId)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.RoomRepository.Delete(room);
                await _unitOfWork.RoomRepository.SaveAsync();
                TempData["success"] = "Room deleted successfully";
                return RedirectToAction("Details", "Hotel", new
                {
                    id = room.HotelId 
                });
            }
            else
            {
                TempData["error"] = "Delete failed, try again";
                return RedirectToAction("Details", "Hotel", new
                {
                    id = room.HotelId
                });
                }
            }
           
        }
    }

           