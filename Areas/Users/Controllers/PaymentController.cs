using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HotelApp.Repository.IRepository;
using HotelApp.Utility;
using HotelApp.Models.Payment;
using HotelApp.Models.Hotel;

namespace HotelApp.Areas.Users.Controllers
{
    [Area("Users")]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IConfiguration configuration, IUnitOfWork unitOfWork, IVnPayService vnPayService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePaymentUrl(int RoomBookingId, double Amount, string OrderDescription, string Name, string UserId)
        {
            var paymentInformationModel = new PaymentInformationModel()
            {
                OrderType = "Booking",
                Amount = Amount,
                OrderDescription = OrderDescription,
                Name = Name,
            };

            var url = _vnPayService.CreatePaymentUrl(paymentInformationModel, HttpContext);
            Console.WriteLine($"Debug - Created Payment URL: {url}");

            HttpContext.Session.SetString("UserId", UserId);
            HttpContext.Session.SetInt32("RoomBookingId", RoomBookingId);

            return Redirect(url);
        }

        [HttpGet]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));

            if (response.Success && response.VnPayResponseCode == "00")
            {
                var payment = new Payment
                {
                    RoomBookingId = HttpContext.Session.GetInt32("RoomBookingId") ?? 0,
                    TotalPrice = double.Parse(response.OrderDescription.Split(' ').Last()),
                    UserId = HttpContext.Session.GetString("UserId"),
                    PaymentTime = DateTime.Now,
                    Status = Payment.PaymentStatus.Success,
                };

                _vnPayService.SavePayment(payment);
                return RedirectToAction("PaymentSuccess");   
            }
            else
            {
                return RedirectToAction("PaymentFailed");
            }
        }

        public async Task<IActionResult> PaymentSuccess()
        {
            return View();
        }

        public async Task<IActionResult> PaymentFailed()
        {
            return View();
        }

    }
}
