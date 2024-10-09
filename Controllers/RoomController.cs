//using HotelApp.Data;
//using HotelApp.Models.Hotel;
//using Microsoft.AspNetCore.Mvc;

//namespace HotelApp.Controllers
//{
//    public class RoomController : Controller
//    {
//        private readonly ApplicationDBContext _dbContext;
//        public RoomController(ApplicationDBContext dbContext)
//        {
//            _dbContext = dbContext;
//        }
//        [Route("Index")]

//        public ActionResult Index(int hotelId)
//        {
//            var rooms = _dbContext.Rooms.Where(r => r.HotelId == hotelId).ToList();
//            return View(rooms);
//        }
//        [Route("Create")]
//        [HttpGet]

//        public IActionResult CreateRoom(int hotelId)
//        {
//            Room room = new Room { HotelId = hotelId };
//            return View(room);
//        }

//        public IActionResult CreateRoom(Room room)
//        {
//            if (ModelState.IsValid)
//            {
//                _dbContext.Rooms.Add(room);
//                _dbContext.SaveChanges();
//                TempData["success"] = "Room created successfully";
//                return RedirectToAction("Index", new { hotelId = room.HotelId });
//            }
//            return View(room);
//        }
//        [Route("Edit")]
//        [HttpGet]

//        public IActionResult EditRoom(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                return NotFound();
//            }
//            Room? room = _dbContext.Rooms.Find(id);
//            if (room == null)
//            {
//                return NotFound();
//            }
//            return View(room);

//        }
//        [Route("Edit")]
//        [HttpPost]

//        public IActionResult EditRoom(Room room)
//        {
//            if (ModelState.IsValid)
//            {
//                _dbContext.Rooms.Update(room);
//                _dbContext.SaveChanges();
//                TempData["success"] = "Updated room successfully";
//                return RedirectToAction("Index", new {hotelId = room.HotelId});
//            }
//            return View();
//        }
//        [Route("Delete")]

//        public IActionResult DeleteRoom(int? id)
//        {
//            if(id == null || id == 0)
//            {
//                return NotFound();
//            }
//            Room? room = _dbContext.Rooms.Find(id);
//            if(room == null)
//            {
//                return NotFound();
//            }
//            return View(room);
//        }
//        [Route("Delete")]
//        [HttpPost]

//        public IActionResult DeleteRoom(Room room)
//        {
//            if (ModelState.IsValid)
//            {
//                _dbContext.Rooms.Remove(room);
//                _dbContext.SaveChanges();
//                TempData["success"] = "Room deleted successfully";
//            }
//            return RedirectToAction("Index", new { hotelId = room.HotelId });
//        }


//    }
//}
