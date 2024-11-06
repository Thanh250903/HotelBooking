using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using HotelApp.Repository;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static HotelApp.Models.Hotel.Room;

namespace HotelApp.Areas.Users.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class BookingController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public BookingController(ApplicationDBContext dbContext, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index(int roomId)
        {
            List<RoomBooking> roombookings = _unitOfWork.BookingRepository.GetBookingByRoomId(roomId).ToList();
            return View(roombookings);
        }
        [HttpGet]
        //Querry search room available
        public IActionResult SearchingAvailableRooms(DateTime checkinDate, DateTime checkoutDate, int hotelId)
        {
            var availableRoom = _dbContext.Rooms.Where(room => !_dbContext.RoomBookings
               .Any(roombooking => roombooking.RoomId == room.RoomId && roombooking.CheckInDate < checkoutDate
               && roombooking.CheckOutDate > checkinDate))
                .Where(room => room.HotelId == hotelId && room.StatusRooms == StatusRoom.Available).ToList();

            return View(availableRoom);
        }

        [HttpGet]
        public async Task<IActionResult> BookingRoom(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomById(id);

            if (room == null)
            {
                return NotFound("Room not found, try again");
            }

            // Checkking room available or not
            if (room.StatusRooms != StatusRoom.Available)
            {
                return NotFound("Room is not available for booking");
            }

            BookingVM bookingVM = new BookingVM
            {
                RoomId = id,
                RoomNumber = room.RoomNumber,
                TotalPrice = room.Price,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
            };

            return View(bookingVM);
        }
        // Handle booking room

        [HttpPost]
        public async Task<IActionResult> BookingRoom(BookingVM bookingVM)
        {
            if (ModelState.IsValid)
            {
                var room = await _unitOfWork.RoomRepository.GetRoomById(bookingVM.RoomId);

                if (room == null)
                {
                    return NotFound("Room not exists");
                }

                if (room.StatusRooms != StatusRoom.Available)
                {
                    return NotFound("Room is not available for booking");
                }
                 // gặp lỗi khi cố gắng thêm User
                //var user = await _userManager.GetUserAsync(User); 
                //if (user == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                //var takeUser = await _userManager.FindByIdAsync(user.Id);
                //if (takeUser == null)
                //{
                //    return NotFound("User not exits");
                //}

                var roomBooking = new RoomBooking
                {
                    RoomId = bookingVM.RoomId,
                    //UserId = user.Id,
                    //User = user, 
                    BookingDate = bookingVM.BookingDate,
                    CheckInDate = bookingVM.CheckInDate,
                    CheckOutDate = bookingVM.CheckOutDate,
                    TotalPrice = bookingVM.TotalPrice,

                };

                _unitOfWork.BookingRepository.Add(roomBooking);
                //room.StatusRooms = StatusRoom.Occupied;
                _unitOfWork.Save();
                TempData["success"] = "Booking a room successfully, next step to confirm";

                return RedirectToAction("AcceptBooking", new
                {
                    bookingId = roomBooking.RoomBookingId
                });

            }
            else
            {
                TempData["error"] = "Try again";
            }
            return View(bookingVM);
        }

        [HttpGet]
        public IActionResult AcceptBooking(int bookingId)
        {
            var booking = _unitOfWork.BookingRepository.Get(acceptbooking => acceptbooking.RoomBookingId == bookingId);
            if (booking == null)
            {
                return NotFound("Cannot find booking's data");
            }
            // access to donfirmbooking
            var confirmbookingVM = new ConfirmBookingVM
            {
                RoomBookingId = booking.RoomBookingId,
                // user will insert it
                FullName = "", 
                Nationality = "",
                PhoneNumber = "",
                Email = "",

            };
            return View(confirmbookingVM);
        }

        [HttpPost]
        public IActionResult AcceptBooking(ConfirmBookingVM confirmBookingVM)
        {
            if(ModelState.IsValid)
            {
                var booking = _unitOfWork.BookingRepository.Get(accpetbooking => accpetbooking.RoomBookingId == confirmBookingVM.RoomBookingId);
                if (booking == null)
                {
                    return RedirectToAction("AccpetBooking", new
                    {
                        bookingId = confirmBookingVM.RoomBookingId,
                    });
                }

                // may not logic, use Identity User
                ConfirmBookingVM confirmBookingVM1 = new ConfirmBookingVM
                {
                    RoomBookingId= confirmBookingVM.RoomBookingId,
                    FullName = confirmBookingVM.FullName,
                    Nationality = confirmBookingVM.Nationality,
                    PhoneNumber = confirmBookingVM.PhoneNumber,
                    Email = confirmBookingVM.Email,
                };
                return RedirectToAction("Payment", "Payment", new 
                { 
                    bookingId = booking.RoomBookingId }
                );

            }
            return View(confirmBookingVM);
        }



       

        // Handle Payment process
        //[HttpPost]
        //public IActionResult Payment (int bookingId, string paymentMethod)
        //{
        //    var booking = _unitOfWork.BookingRepository.FirstOrDefault(roombooking => roombooking.RoomBookingId != bookingId);
        //    if(booking == null || booking.PaymentStatus == PaymentSatus.Paid) 
        //    {
        //        return NotFound("Cannot handle payment");
        //    }
        //    var payment = new Payment
        //    {
        //        RoomBookingId = bookingId,
        //        PaymentMethod = paymentMethod,
        //        Amount = booking.TotalPrice,
        //        PaymentDate = DateTime.Now
        //    };

        //    booking.PaymentStatus = "Paid";
        //    booking.BookingStatus = "Confirmed";

        //    _context.Payments.Add(payment);
        //    _context.SaveChanges();

        //    return RedirectToAction("BookingDetails", new { bookingId = bookingId });
        //}


    }
}

