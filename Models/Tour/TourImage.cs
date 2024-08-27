using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Tour
{
    // đại diện cho ảnh về chuyến du lịch
    public class TourImage
    {
        [Key] 
        public int TourImageId { get; set; } // mã ảnh của chuyến đi
        [Required]
        public int TourId { get; set; } // mã định danh của chuyến đi
        [Required]
        [ForeignKey("TourId")]
        public Tour Tour { get; set; } // liên kết với Tour, xem ảnh này thuộc Tour nào
        [Required]
        public string ImageUrl { get; set; } // ảnh của Tour
    }
}
