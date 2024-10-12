using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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
        [Route("Edit/id")]
        public IActionResult EditRoom(int id)
        {
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                return NotFound();
            }

            RoomVM roomVM = new RoomVM
            {
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
        [Route("Edit/id")]
        public IActionResult EditRoom(RoomVM roomVM)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room
                {
                    RoomId = roomVM.RoomId,
                    RoomNumber = roomVM.RoomNumber,
                    RoomType = roomVM.RoomType,
                    Price = roomVM.Price,
                    StatusRooms = roomVM.StatusRoom,
                    BedCount = roomVM.BedCount,
                    RoomImgUrl = roomVM.RoomImgUrl,
                };
                _unitOfWork.RoomRepository.Update(room);
                _unitOfWork.Save();
                TempData["success"] = "Edit room successfully";
                TempData["ShowMessage"] = true;
                return RedirectToAction("Index", new
                {
                    id = roomVM.HotelId
                });
            }
            return View(roomVM);
        }

        [HttpGet]
        [Route("Delete/id")]
        public IActionResult DeleteRoom(int id)
        {
            Room room = new Room();
            room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        [Route("Delete/id")]
        public IActionResult DeleteRoom(int id, Room room)
        {
            if (ModelState.IsValid)
            {
                room = _unitOfWork.RoomRepository.GetById(id);
                if (room != null)
                {
                    _unitOfWork.RoomRepository.Delete(room);
                    _unitOfWork.Save();
                    TempData["success"] = "Delete room successfully";
                    TempData["ShowMessage"] = true;
                }
            }
            return RedirectToAction("Index", new
            {
                id = room.HotelId
            });
        }
    }
}
