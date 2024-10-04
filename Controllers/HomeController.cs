using HotelApp.Data;
using HotelApp.Models;
using HotelApp.Repository.IRepository;
using HotelApp.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HotelApp.Repository;


namespace HotelApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHotelRepository HotelRepository;
		private readonly ILogger<HomeController> _logger;
		private ApplicationDBContext _dbContext;
		

		public HomeController(ILogger<HomeController> logger, IHotelRepository hotelRepository, ApplicationDBContext dBContext)
		{
			_logger = logger;
			HotelRepository = hotelRepository;
			_dbContext = dBContext;
		}

		public IActionResult Index()
		{
			List<Hotel> hotels = _dbContext.Hotels.ToList();
			return View(hotels);
		}
		public IActionResult Details()
		{
			return View();
		}
		public IActionResult CreateHotel()
		{
			return View();
		}
		public IActionResult AboutUs()
		{
			return View();
		}
		public IActionResult Services()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateHotel(Hotel hotel)
		{
			if (ModelState.IsValid)
			{
				_dbContext.Hotels.Add(hotel);
				_dbContext.SaveChanges();
				TempData["success"] = "Create hotel successfully";
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
			Hotel? hotel = _dbContext.Hotels.Find(id);
			if (hotel == null)
			{
				return NotFound();
			}
			return View(hotel);
		}

		[HttpPost]
		public IActionResult EditHotel(Hotel hotel)
		{
			if (ModelState.IsValid)
			{
				_dbContext.Hotels.Update(hotel);
				_dbContext.SaveChanges();
				TempData["success"] = "Hotel edit successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult DeleteHotel(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Hotel? hotel = _dbContext.Hotels.Find(id);
			if (hotel == null)
			{
				return NotFound();
			}
			return View(hotel);
		}

		[HttpPost]
		public IActionResult DeleteHotel(Hotel hotel)
		{
			_dbContext.SaveChanges();
			_dbContext.Hotels.Remove(hotel);
			TempData["success"] = "Hotel delete successfully";
			return RedirectToAction("Index");
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
