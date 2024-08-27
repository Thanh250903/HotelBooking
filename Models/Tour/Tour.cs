using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Tour
{
    // đại diện cho thông tin về chuyến du lịch
    public class Tour
    {
        [Key]
        public int TourId{ get; set; } // mã định danh của chuyến du lịch
        [Required]
        public string TourName { get; set; } // tên của chuyến du lịch
        [Required]
        public string Description { get; set; } // miêu tả về chuyến du lịch
        [Required]
        public string Address { get; set; } // địa điểm du lịch
        [NotMapped] // kh lưu được ở trong CSDL và khi thêm khách sạn sẽ kh hiện trường này lên, nó sẽ lấy dữ liệu từ HotelReview
        public double AverangeRating
        {
            get
            {
                if (TourReviews != null && TourReviews.Any())
                {
                    return TourReviews.Average(review => review.Rating);
                }
                return 0; // Giá trị mặc định khi không có đánh giá nào
            }
        } // trường này đùng để tính toán số lượng Review đánh giá về khách sạn, không lưu trữ trong Database.
        [Required]
        [DataType(DataType.Currency)] // attributes kiểu tiền tệ
        public decimal TicketPrice { get; set; } // lưu giá vé chung, vé chỉ có 1 giá nếu đặt trong Tourbooking giá trị sẽ kh cụ thểm thay đổi liên tục
        [Required]
        public TimeOnly OpeningHours { get; set; } // giờ mở cửa
        [Required]
        public TimeOnly CloseHour { get; set; } // giờ đóng cửa
        [Required]
        public double Latitube { get; set; } // kinh độ
        [Required]
        public double Longitude { get; set; } // vĩ độ
        public ICollection<TourBooking>? TourBookings {  get; set; } // điều hướng dữ liệu tới TourBooking
        public ICollection<TourImage>? TourImages { get; set; } // điều hướng tới dữ liệu TourImage
        public ICollection<TourReview>? TourReviews { get; set; } // điều hướng tới dữ liệu TourReview
    }
}
