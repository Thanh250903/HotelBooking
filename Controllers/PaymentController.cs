using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Others;
using HotelApp.Repository;
using HotelApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public PaymentController(ApplicationDBContext dBContext, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
        {
            _dbContext = dBContext;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }
        // lấy id của booking để tiến hành payment, lưu ý không nên dùng list
        public IActionResult Index(int bookingId)
        {
            return View(bookingId);
        }
        public IActionResult Payment()
        {
            return View();
        }

    }
}
