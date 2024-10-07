using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace HotelApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelRepository HotelRepository;

        private ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HotelController(IHotelRepository hotelRepository, ApplicationDBContext dbContext, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            HotelRepository = hotelRepository;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<IActionResult> CreateHotel(Hotel hotel, IFormFile? imageFile)
        {
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
                    //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    //string hotelPath = Path.Combine(wwwRootPath + "images/hotels", fileName);

                    // Save image to path 
                    //using (var fileStream = new FileStream(hotelPath, FileMode.Create))
                    //{
                    //    await imageFile.CopyToAsync(fileStream);
                    //}
                    //hotel.ImageUrl = "/images/hotels/" + fileName;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    hotel.ImageUrl = $"/img/" + fileName;

                }  
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
