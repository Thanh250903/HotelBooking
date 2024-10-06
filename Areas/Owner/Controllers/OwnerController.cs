using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Owner.Controllers
{
    // Owner sẽ có HotelController: Quản lý các khách sạn do họ đăng ký => Method: ListHotel, ViewHotelDetail, CreateHote, EditHotel, ViewHotel, DeleteHotel, ViewBookingRoom 
    // Owner sẽ có RoomController: Quản lý các phòng trong khách sạn của họ => Method: ListRoom, ListRoomDetails,CreateRoom, EditRoom, ViewRoom, DeleteRoom, Search Room, SetRoomAvailability, BlockRoom
    // Owner sẽ có BookingManagementController: Xem và quản lý các booking mà chủ khách sạn sở hữu => Method: ListBookings, ViewBookingDetails, Checkin, ViewRoomBooking, 
    // Owner sẽ có ProfileController: Xem và quản lý thông tin cá nhân của chủ sở hữu => Method: ViewProfile, EditProfile, ChangePassword, UploadProfilePicture
    // Owner sẽ có PaymenController: Quản lý và xem các thanh toán mà chủ sở hữu nhận được => ViewPayment,  Receive Payment, ViewReceivePayment, ViewPaymenDetail, SearchPayment
    public class OwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
