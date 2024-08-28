using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Admin.Controllers
{
    // Admin sẽ có HotelManagementController: Dùng để quản lý các khách sạn trong hệ thống => Method: ListHotel, ViewHotelDetail, ApproveHotel, RejectHotel, SearchHotel, ListPendingHotel
    // Admin sẽ có TourManagementController: Dùng để quản lý các Tour du lịch trong hệ thống => Method: ListTour, ViewTourDetail, ApproveTour, RejectTour, SearchTour, LisPendingTour
    public class HotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
