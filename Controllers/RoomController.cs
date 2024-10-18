using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomController(ApplicationDBContext dbContext, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
        [Route("CreateRoom")]
        public IActionResult CreateRoom(int id)
        {
            //AppDomain/CreateRoom?id=23&searchTerm="ưèwè"
            // get hotel id from route
            var hotel = _unitOfWork.HotelRepository.GetById(id);
            if(hotel == null)
            {
                return NotFound("Cannot find the hotel to create room");
            }
            RoomVM roomVM = new RoomVM
            {
                HotelId = id,
                HotelName = hotel.HotelName,
            };
            return View(roomVM);
        }

        [HttpPost]
        [Route("CreateRoom")]
        public async Task<IActionResult> CreateRoom(RoomVM roomVM, IFormFile? roomImages)
        {
            
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
                var hotelExists = _unitOfWork.HotelRepository.GetById(room.HotelId);
                if (hotelExists == null)
                {
                    ModelState.AddModelError("", "Invalid HotelId");
                    return View(roomVM);
                }

                //Check roomnumber same name
                if(!_unitOfWork.RoomRepository.IsRoomNumberUnique(roomVM.HotelId, roomVM.RoomNumber))
                {
                    ModelState.AddModelError("", "Room number already exits for this hotel, try again!!!");
                    return View(roomVM);
                }

                //Add room into hotel method
                _unitOfWork.RoomRepository.Add(room);
                _unitOfWork.Save();
                TempData["success"] = "Add room successfully";// bool

                // bool saveresult =  _unitOfWork.Save();
                TempData["ShowMessage"] = true;

                return RedirectToAction("Details", "Hotel", new
                {
                    id = roomVM.HotelId
                });
            }
            else
            {
                TempData["error"] = "Try again";
            }
            return View(roomVM);
        }

        [HttpGet]
        public IActionResult EditRoom(int id)
        {
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                return NotFound();
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
            if (ModelState.IsValid)
            {
                // Handle image update
                if(roomImages != null && roomImages.Length > 0)
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
                
                var room = _unitOfWork.RoomRepository.GetById(roomVM.RoomId);
                if(room == null)
                {
                    ModelState.AddModelError("", "Room not found");
                    return View(roomVM);
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

                _unitOfWork.RoomRepository.Update(room);
                _unitOfWork.Save();
                TempData["success"] = "Edit room successfully";
                TempData["ShowMessage"] = true;
                return RedirectToAction("Details", "Hotel", new
                {
                    id = roomVM.HotelId
                });
            }
            return View(roomVM);
        }

        [HttpGet, HttpPost]
        public IActionResult DeleteRoom(int id)
        {
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                TempData["error"] = "Room not found. Deletion failed.";
                return RedirectToAction("Index", "Hotel");
            }

            if (Request.Method == "GET")
            {
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
            else if (Request.Method == "POST")
            {
                try
                {
                    int hotelId = room.HotelId; // Lấy HotelId trước khi xóa

                    _unitOfWork.RoomRepository.Delete(room);
                    _unitOfWork.Save();

                    TempData["success"] = "Room deleted successfully";
                    return RedirectToAction("Details", "Hotel", new { id = hotelId });
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine("Error deleting room: " + ex.Message);
                    TempData["error"] = "An error occurred while trying to delete the room. Please try again.";
                    return RedirectToAction("Details", "Hotel", new { id = room.HotelId });
                }
            }

            TempData["error"] = "Invalid request method.";
            return RedirectToAction("Index", "Hotel");
        }
    }
}
