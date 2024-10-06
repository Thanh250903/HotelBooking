
using HotelApp.Models.Hotel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Others
{
    public class Payment // model này đại diện cho các phương thức thanh toán, ở đây chúng tôi chỉ thanh toán onlnie qua các hình thức như BankTransfer, CreditCard và Momo
    {
        [Key]
        public int PaymentId { get; set; } // mã của thanh toán 
        [Required]
        public int UserId { get; set; } // mã khách hàng
        [Required]
        [ForeignKey("UserId")] 
        public ApplicationUser User { get; set; } // liên kết với khóa ngoại để bt khách hàng nào
        [Required]
        public int? RoomBookingId { get; set; } // mã thuê phòng
        [ForeignKey("RoomId")]
        public RoomBooking? RoomBooking { get; set; } // liên kết với khóa ngoại để bt thuê phòng nào
        [Required]
        public DateTime PaymentTime { get; set; } // thời gian thanh toán
        [Required]
        [DataType(DataType.Currency)] // attributes kiểu tiền tệ
        public decimal TotalPrice { get; set; } // tổng giá
        public enum PaymenMethod
        {
            BankTransfer, // ck qua ngân hàng
            CreditCard, // Bao gồm thẻ Visa, MasterCard, vv
            Momo
        }
        public enum PaymentStatus // tình trạng thanh toán
        {
            Paid,
            Pending,
            Failed
        }
        public PaymenMethod Method { get; set; }
        public enum BookingType // đặt phòng hay là chuyến đi
        {
            Room
        }
        [Required]
        public BookingType Status { get; set; }
    }
}
