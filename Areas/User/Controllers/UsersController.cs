using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.User.Controllers
{
    // User sẽ có BookingController: Cho phép người dùng đặt phòng => Method:  CreateBooking, ViewBookingDetail, CancelBooking, ViewBookingHistory, Search hotel
    // User sẽ có ReviewController: Cho phép người dùng đánh giá các khách sạn khác => Method: CreateReview, ViewReview, EditReview, DeleteReview, ViewHotelReviews, RateReview
    // User sẽ có ProfileController: Cho phép người dùng chỉnh sửa thông tin cá nhân, cập nhật profile của họ => Method: ViewProfile, EditProfile, ChangePassword, UploadProfilePicture, DeleteProfile
    // User sẽ có TourBookingController: Cho phép người dùng tìm kiếm và đặt chỗ cho chuyến đi của họ Method: => ViewTourDetails, SearchTour, BookingTour, ViewTourBookingHistory, CancelBooking
    // User sẽ có PaymenController: Cho phép người dùng thanh toán phòng, chuyến đi => Method: MakePayment, ViewPaymentHistory, ViewPaymentDetail
    // User sẽ có AddToCartController: Cho phép người dùng bấm vào giỏ hàng để xem khách sạn, chuyến đi nào mình đã thích => Method: AddToCart, ViewCart, RemoveFromCart, ClearCart
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
