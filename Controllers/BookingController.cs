using HotelApp.Data;
using HotelApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UnitOfWork _unitOfWork;
        public BookingController(ApplicationDBContext dbContext, UnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BookingRoom()
        {
            return View();
        }
        public IActionResult SubmitBooking()
        {
            return View();
        }
    }
}
