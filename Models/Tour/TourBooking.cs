using HotelApp.Models.Others;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Tour
{
    // đại diện cho thông tin chuyến đi du lịch
    public class TourBooking
    {
        [Key]
        public int TourBookingId { get; set; } // mã định danh đặt chuyến đi
        [Required]
        public int TourId { get; set; } // khóa ngoại tham chiếu tới Tour để bt địa điểm tham quan của khách
        [Required]
        [ForeignKey("TourId")]
        public Tour Tour { get; set; } // liên kết với Tour xem TourBooking này thuộc chuyến đi nào
        [Required]
        public int UserId { get; set; } // tham chiếu khóa ngoại để bt User nào book chuyến đi này
        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } // liên kết với User xem TourBooking này là khách hàng nào đã đặt nó
        [Required]
        public DateTime BookingDate { get; set; } // lưu trữ ngày, h đặt chỗ
        [Required]
        public DateTime VisitDate { get; set; } // dự kiến ngày tham quan
        [Required]
        public int NumberOfTickets { get; set; } // số lượng vé
        [Required]
        public decimal TotalPrice
        {
            get
            {
                return NumberOfTickets * Tour.TicketPrice;
            }
        } // tùy vào số lượng vé, giá sẽ tăng tên

        //[NotMapped]
        //public decimal TicketPrice => Attractions.TicketPrice; // lấy giá vé từ bảng Attractions
        //[NotMapped]
        //public decimal AttractionTotalPrices => TicketPrice * NumberOfTickets; // tổng giá vé dựa trên số lượng vé
        public int? PaymentId { get; set; } // null nếu kh có thanh toán
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; } // thông tin liên quan đến đặt chỗ của chuyến du lịch





    }
}
