using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Hotel
{
    // đại diện cho ảnh của khách sạn
    public class HotelImage
    {
        [Key]
        public int HotelImageId { get; set; } // mã định danh của ảnh
        [Required]
        public int HotelId { get; set; } // mã định danh của khách sạn, xem ảnh này thuộc khách sạn nào
        [Required]
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; } // liên kết với khách sạn, xem ảnh này thuộc khách sạn nào
        [Required]
        public string UrlImage { get; set; } // Url của ảnh
    }
}
