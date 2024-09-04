using HotelApp.Data;
using HotelApp.Models.Hotel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Admin.Controllers
{
    // Admin sẽ có HotelManagementController: Dùng để quản lý các khách sạn trong hệ thống => Method: ListHotel, ViewHotelDetail, ApproveHotel, RejectHotel, SearchHotel, ListPendingHotel
    // Admin sẽ có TourManagementController: Dùng để quản lý các Tour du lịch trong hệ thống => Method: ListTour, ViewTourDetail, ApproveTour, RejectTour, SearchTour, LisPendingTour
    public class HotelController : Controller
    {
       private readonly ApplicationDBContext _dbContext;
       private readonly UserManager<IdentityUser> _userManager;
       
       public HotelController(ApplicationDBContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
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
        public IActionResult CreateHotel ()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateHotel(Hotel hotel) 
        {
            _dbContext.Hotels.Add(hotel);
            _dbContext.SaveChanges();
            return View(hotel);
        }
    }
}
