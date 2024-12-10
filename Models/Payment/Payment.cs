using HotelApp.Models.Hotel;
using HotelApp.Models.Others;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Payment
{
    public class Payment // model này đại diện cho các phương thức thanh toán, ở đây chúng tôi chỉ thanh toán onlnie qua các hình thức như BankTransfer, CreditCard và Momo
    {
        public int PaymentId { get; set; }
        public double TotalPrice { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ValidateNever]
        public int RoomBookingId { get; set; }

        [ForeignKey("RoomBookingId")]
        [ValidateNever]
        public RoomBooking RoomBooking { get; set; }

        [ValidateNever]
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        [Required]
        public PaymentStatus Status { get; set; }
        public enum PaymentStatus
        {
            Success,
            Faileded
        }
    }
}
