using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Others;
using HotelApp.Repository;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;
using static HotelApp.Models.Hotel.Room;

namespace HotelApp.Areas.Users.Controllers
{
    [Area("Users")]
    //[Authorize(Roles = "User")]
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
        //[HttpGet]
        ////Querry search room available
        //public IActionResult SearchingAvailableRooms(DateTime checkinDate, DateTime checkoutDate, int hotelId)
        //{
        //    var availableRoom = _dbContext.Rooms.Where(room => !_dbContext.RoomBookings
        //       .Any(roombooking => roombooking.RoomId == room.RoomId && roombooking.CheckInDate < checkoutDate
        //       && roombooking.CheckOutDate > checkinDate))
        //        .Where(room => room.HotelId == hotelId && room.StatusRooms == StatusRoom.Available).ToList();

        //    return View(availableRoom);
        //}

        [HttpGet]
        public async Task<IActionResult> BookingRoom(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["error"] = "You must login to the system to booking a room";
                return RedirectToAction("Login", "Account");
            }

            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(id);

            if (room == null)
            {
                TempData["error"] = "Room not exists";
                return NotFound();
            }

            // Checkking room available or not
            if (room.StatusRooms != StatusRoom.Available)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User); 
            if (user == null) 
            {
                return RedirectToAction("Login", "Account"); 
            }
            // use BookingVM that contain UserId, RoomId, Price to create new Booking Room 

            BookingVM bookingVM = new BookingVM
            {
                UserId = user.Id,
                RoomId = id,
                HotelId = room.HotelId,
                RoomNumber = room.RoomNumber,
                Price = room.Price,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
                TotalPrice = room.Price
            };

            return View(bookingVM);
        }
        // Handle booking room

        [HttpPost]
        public async Task<IActionResult> BookingRoom(BookingVM bookingVM)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                TempData["error"] = "Invalid data. Please check the input.";
                return View(bookingVM);
            }

            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(bookingVM.RoomId);

            if (room == null)
            {
                TempData["error"] = "Room is not available for booking";
                return NotFound();
            }

            if (room.StatusRooms != StatusRoom.Available)
            {
                TempData["error"] = "Room is not available for booking";
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = await _userManager.FindByIdAsync(user.Id);
            if (userId == null)
            {
                TempData["error"] = "User not found";
                return NotFound();
            }

            // Set UserId in bookingVM
            bookingVM.UserId = user.Id;

            //calculating price each night
            int priceEachNight = (bookingVM.CheckOutDate - bookingVM.CheckInDate).Days;
            bookingVM.TotalPrice = priceEachNight * bookingVM.Price;

            var roomBooking = new RoomBooking
            {
                RoomId = bookingVM.RoomId,
                UserId = bookingVM.UserId,
                User = userId,
                BookingDate = bookingVM.BookingDate,
                CheckInDate = bookingVM.CheckInDate,
                CheckOutDate = bookingVM.CheckOutDate,
                TotalPrice = bookingVM.TotalPrice,
                PricePerNight = bookingVM.Price,
                IsPaid = true,
            };

            _unitOfWork.BookingRepository.Add(roomBooking);
            room.StatusRooms = StatusRoom.Occupied;
            _unitOfWork.Save();
            TempData["success"] = "Booking a room successfully, click next to confirm";
            return RedirectToAction("AcceptBooking", new
            {
                bookingId = roomBooking.RoomBookingId
            });
        }

        [HttpGet]
        public IActionResult AcceptBooking(int bookingId)
        {
            var booking = _unitOfWork.BookingRepository.Get(acceptbooking => acceptbooking.RoomBookingId == bookingId);
            if (booking == null)
            {
                return NotFound("Cannot find booking's data");
            }

            var confirmbookingVM = new ConfirmBookingVM
            {
                RoomBookingId = booking.RoomBookingId,
                FullName = "",
                Nationality = "",
                PhoneNumber = "",
                Email = "",
            };
            return View(confirmbookingVM);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptBooking(ConfirmBookingVM confirmBookingVM)
        {
            if (ModelState.IsValid)
            {
                var booking = _unitOfWork.BookingRepository.Get(accpetbooking => accpetbooking.RoomBookingId == confirmBookingVM.RoomBookingId);
                if (booking == null)
                {
                    return RedirectToAction("AcceptBooking", new
                    {
                        bookingId = confirmBookingVM.RoomBookingId,
                    });
                }
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account"); 
                }

                ViewBag.RoomBookingId = booking.RoomBookingId;
                ViewBag.UserId = user.Id;

                return RedirectToAction("CreatePaymentUrl", "Payment", new
                {
                    RoomBookingId = booking.RoomBookingId,
                    Amount = booking.TotalPrice,
                    OrderDescription = "Pay ment for hotel",
                    Name = user.UserName,
                    UserId = user.Id
                });
            }
            return View(confirmBookingVM);
        }


    }
}

