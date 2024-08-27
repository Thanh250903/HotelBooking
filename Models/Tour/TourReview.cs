using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Tour
{
    public class TourReview
    {
        // đại diện đánh giá của khách hàng về chuyến du lịch
        [Key]
        public int ReviewId { get; set; } // mã định danh của Review
        [Required]
        public int TourId { get; set; } // khóa ngoại tham chiếu tới Tour để bt địa điểm tham quan của khách
        [Required]
        [ForeignKey("TourId")]
        public Tour Tour { get; set; } // liên kết với Tour xem Review này thuộc Tour nào
        [Required]
        [Range(1,5)]
        public int Rating { get; set; } // đánh giá từ 1 đến 5 sao
        [Required]
        [StringLength(100)]
        public string Comment { get; set; } // comment điểm đến
        [Required]
        public string UserName { get; set; } // tên người đánh giá
    }
}
