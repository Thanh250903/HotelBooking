using HotelApp.Models.Hotel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Others
{
    public class Payment // model này đại diện cho các phương thức thanh toán, ở đây chúng tôi chỉ thanh toán onlnie qua các hình thức như BankTransfer, CreditCard và Momo
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ValidateNever]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int RoomBookingId { get; set; }

        [ForeignKey("RoomBookingId")]
        [ValidateNever]
        public RoomBooking RoomBooking { get; set; }

        [Required]
        [ValidateNever]
        public DateTime PaymentTime { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Currency)]
        [ValidateNever]
        public decimal TotalPrice { get; set; }

        public enum PaymentMethod
        {
            VNPay,
            PayPal,
            Stripe
        }

        public enum PaymentStatus
        {
            Paid,
            Pending,
            Unpaid
        }

        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        public string TransactionId { get; set; }

        public string PaymentResponse { get; set; }
    }
}
