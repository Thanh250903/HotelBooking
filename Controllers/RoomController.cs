using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public RoomController(ApplicationDBContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        [Route("Index")]
        // Display list room of hotel

        public IActionResult Index(int hotelId)
        {
            var rooms = _unitOfWork.RoomRepository.GetRoomsByHotelId(hotelId);
            ViewBag.HotelId = hotelId;
            return View(rooms);
        }
        [Route("Detail")]

        public IActionResult Details(int id)
        {
            var room = _unitOfWork.RoomRepository.GetById(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        [Route("Create")]
        [HttpGet]

        public IActionResult CreateRoom(int hotelId)
        {
            Room room = new Room 
            { 
                HotelId = hotelId
            };
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Rooms.Add(room);
                _dbContext.SaveChanges();
                TempData["success"] = "Room created successfully";
                return RedirectToAction("Index", new 
                { 
                    hotelId = room.HotelId 
                });
            }
            return View(room);
        }
        [Route("Edit")]
        [HttpGet]

        public IActionResult EditRoom(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Room? room = _dbContext.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);

        }
        [Route("Edit")]
        [HttpPost]

        public IActionResult EditRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Rooms.Update(room);
                _dbContext.SaveChanges();
                TempData["success"] = "Updated room successfully";
                return RedirectToAction("Index", new { hotelId = room.HotelId });
            }
            return View();
        }
        [Route("Delete")]

        public IActionResult DeleteRoom(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Room? room = _dbContext.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        [Route("Delete")]
        [HttpPost]

        public IActionResult DeleteRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Rooms.Remove(room);
                _dbContext.SaveChanges();
                TempData["success"] = "Room deleted successfully";
            }
            return RedirectToAction("Index", new { hotelId = room.HotelId });
        }


    }
}
