
using HotelApp.Models.Hotel.VM;
using HotelApp.Models.Payment;

namespace HotelApp.Utility
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
        void SavePayment(Payment payment);
    }
}
