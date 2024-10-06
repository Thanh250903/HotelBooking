using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelRepository HotelRepository;

        private ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public HotelController(IHotelRepository hotelRepository, ApplicationDBContext dbContext, IUnitOfWork unitOfWork)
        {
            HotelRepository = hotelRepository;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            List<Hotel> hotels = _unitOfWork.HotelRepository.GetAll().ToList();
            return View(hotels);
        }
        [Route("Details")]
        public IActionResult Details()
        {
            return View();
        }

        [Route("Create")]
        public IActionResult CreateHotel()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]

        public IActionResult CreateHotel(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.HotelRepository.Add(hotel);
                _unitOfWork.Save();
                TempData["CreateHotel"] = "Hotel created successfully";
                TempData["ShowMessage"] = true;
                return RedirectToAction("Index");
            }
            return View();
        }
        [Route("Edit/id")]

        public IActionResult EditHotel(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Hotel? hotel =_unitOfWork.HotelRepository.Get(h => h.HotelId == id);
            if (hotel == null)
            {
                NotFound();
            }
            return View(hotel);
        }
        [Route("Edit/id")]
        [HttpPost]

        public IActionResult EditHotel(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.HotelRepository.Update(hotel);
                _unitOfWork.Save();
                TempData["success"] = "Hotel Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [Route("Delete/id")]

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
        [Route("Delete/id")]
        [HttpPost]

        public IActionResult DeleteHotel(Hotel hotel)
        {
            _unitOfWork.HotelRepository.Delete(hotel);
            _unitOfWork.Save();
            TempData["DeleteHotel"] = "Hotel deleted successfully";
            TempData["ShowMessage"] = true;
            return RedirectToAction("Index");
        }
    }
}
