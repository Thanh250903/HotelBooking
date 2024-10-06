using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Staff.Controllers
{
    // Staff sẽ có NotificationController: Để Quản lý gửi thông báo về booking cho người dùng => Method: ListNotification, SendNotification, GetNotification, ResendNotification
    // Staff sẽ có CheckInReminderController: Để nhắc nhở người dùng tới ngày nhận phòng => Method: ListReminders, SendReminders,ScheduleReminder, EditReminder, DeleteReminder
    // Staff sẽ có RoomServiceController: Để quản lý phòng trống, phòng được booking => Method: ListRoomServe, ViewServiceRequests, ListEmptyRoom, ListBookingRoom, ListPendingRoom
    // Staff sẽ có ProfileController: Để quản lý thông tin cá nhân, cập nhật chúng => ViewProfile, EditProfile, ChangePassWord, UploadProfilePicture
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
