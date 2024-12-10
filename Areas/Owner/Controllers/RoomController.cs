using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static HotelApp.Models.Hotel.Room;

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
                TempData["error"] = "You don't have premission to create a room";
                return Unauthorized();
            }

            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                TempData["error"] = "Cannot find the hotel to create room";
                return NotFound("");
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
                TempData["error"] = "You don't have premission to create room";
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

                // check if Hotel Exists or not
                var hotelExists = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
                if (hotelExists == null)
                {
                    TempData["error"] = "Hotel not exists";
                    return View(roomVM);
                }

                //Check roomnumber same name or not
                if (!await _unitOfWork.RoomRepository.IsRoomNumberUniqueAsync(roomVM.HotelId, roomVM.RoomNumber))
                {
                    TempData["error"] = "This room exists, create other room number";
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
            // Detail of room
            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            // checking owner or not
            var ownerId = _userManager.GetUserId(User);
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
            if (hotel == null || hotel.OwnerId != ownerId)
            {
                TempData["error"] = "You do not have permission to edit this room.";
                return Unauthorized("");
            }

            // Create VM to add Room
            var roomVM = new RoomVM
            {
                HotelId = room.HotelId,
                HotelName = hotel.HotelName, 
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Price = room.Price,
                StatusRoom = room.StatusRooms,
                BedCount = room.BedCount,
                RoomImgUrl = room.RoomImgUrl,
                IsOwner = hotel.OwnerId == ownerId 
            };

            return View(roomVM);
        }


        [HttpPost]
        public async Task<IActionResult> EditRoom(RoomVM roomVM, IFormFile? roomImages)
        {
            var ownerId = _userManager.GetUserId(User);

            // Checking owner is own this hotel or not
            if (!await _unitOfWork.HotelRepository.IsHotelOwnerAsync(roomVM.HotelId, ownerId))
            {
                TempData["error"] = "You do not have permission to edit this room.";
                return Unauthorized();
            }

            // Checking input data
            if (roomVM == null)
            {
                TempData["error"] = "Room invalid";
                return View(roomVM);
            }

            if (!ModelState.IsValid)
            {
                return View(roomVM);
            }

            try
            {
                // Take a room to edit, return room not found if the room not exits
                var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(roomVM.RoomId);
                if (room == null)
                {
                    TempData["error"] = "Room not found";
                    return View(roomVM);
                }

                // update image
                if (roomImages != null && roomImages.Length > 0)
                {
                    var wwwRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var fileName = Path.GetFileName(roomImages.FileName);
                    var filePath = Path.Combine(wwwRootPath, fileName);

                    // load image
                    if (!Directory.Exists(wwwRootPath))
                    {
                        Directory.CreateDirectory(wwwRootPath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await roomImages.CopyToAsync(fileStream);
                    }
                    roomVM.RoomImgUrl = $"/img/{fileName}";
                }

                // Only room
                if (!_unitOfWork.RoomRepository.IsRoomNumberUnique(roomVM.HotelId, roomVM.RoomNumber))
                {
                    TempData["error"] = "Room number already exists";
                    return View(roomVM);
                }

                // Update room information
                room.RoomNumber = roomVM.RoomNumber;
                room.RoomType = roomVM.RoomType;
                room.Price = roomVM.Price;
                room.StatusRooms = roomVM.StatusRoom;
                room.BedCount = roomVM.BedCount;
                room.RoomImgUrl = roomVM.RoomImgUrl;

                _unitOfWork.RoomRepository.Update(room);
                await _unitOfWork.RoomRepository.SaveAsync();

                TempData["success"] = "Edit room successfully.";
                return RedirectToAction("Details", "Hotel", new { id = roomVM.HotelId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(roomVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            // checking room exits or not
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                TempData["error"] = "Room not found, deleted failed.";
                return RedirectToAction("Index", "Hotel");
            }
            // checking the owner that own this hotel or not
            var ownerId = _userManager.GetUserId(User);
            var hotel = await _unitOfWork.HotelRepository.GetHotelByIdAsync(room.HotelId);
            if (hotel.OwnerId != ownerId)
            {
                TempData["error"] = "You do not have permission to Delete.";
                return Unauthorized();
            }
            // create roomVM to display detail of room before delete
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
            // checking room exits or not
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
                TempData["error"] = "You do not have permission to delete this room.";
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

        [HttpGet]
        public IActionResult RoomRented(int hotelId)
        {
            // display all room have the same hotel Id with status is Occupied
            var rooms = _unitOfWork.RoomRepository.GetAll(room =>
                        room.HotelId == hotelId && room.StatusRooms == StatusRoom.Occupied).ToList();
            var roomVMs = rooms.Select(room => new RoomVM
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

            ViewBag.Hotels = _unitOfWork.HotelRepository.GetAll();
            return View(roomVMs);
        }

        [HttpGet]
        [Route("Owner/Room/CustomerReturnRoom/{id}")]
        public IActionResult CustomerReturnRoom(int id)
        {
            // Finding a room by Id
            var room = _unitOfWork.RoomRepository.Get(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            // Change status of room
            room.StatusRooms = StatusRoom.Available;
            _unitOfWork.Save();

            TempData["success"] = "Room returned successfully!";
            return RedirectToAction("Details", "Hotel", new {
                id = room.HotelId

            });
        }

    }

}

           