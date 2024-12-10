using HotelApp.Models.Others;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Hotel
{
    public class HotelReview
    {
        // đại diện đánh giá cho khách hàng về khách sạn
        [Key]
        public int ReviewId { get; set; } 
        [ValidateNever]
        public int HotelId { get; set; } 
        [ValidateNever]
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; } 
        [ValidateNever]
        [Range(1,5)]
        public string? UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Required(ErrorMessage = "Comment is required.")]
        [StringLength(1000, ErrorMessage = "Comment cannot be longer than 1000 characters.")]
        public string Comment { get; set; } // bình luận của người dùng
        public DateTime CreateAt { get; set; } // ngày đánh giá
        public DateTime? LastEditAt { get; set; } // ngày cuối chỉnh sửa
        public string? Image {  get; set; } // hình ảnh đại diện cho Comment
    }
}
