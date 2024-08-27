using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Hotel
{
    public class HotelReview
    {
        // đại diện đánh giá cho khách hàng về khách sạn
        [Key]
        public int ReviewId { get; set; } // mã định danh của Review khách sạn
        [Required]
        public int HotelId { get; set; } // mã định danh của ksan, để biết Review này thuộc ksan nào
        [Required]
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; } // liên kết với khóa ngoại của Hotel
        [Required]
        [Range(1,5)]
        public int Rating { get; set; } // đánh giá từ 1 đến 5 sao
        [Required]
        [StringLength(100)]
        public string Comment { get; set; } // bình luận của người dùng
        [Required]
        public string UserName { get; set; } // tên người bình luận
    }
}
