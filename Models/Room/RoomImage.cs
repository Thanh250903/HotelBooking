using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Security.Policy;

namespace HotelApp.Models.Room
{
    // đại diện cho hình ảnh của phòng  
    public class RoomImage
    {
        [Key]
        public int RoomImageId { get; set; } // mã định danh của ảnh
        [Required]
        public int RoomId { get; set; } // mã định danh của phòng, xem ảnh này thuộc phòng nào
        [Required]
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // liên kết với phòng, xem ảnh này thuộc phòng nào
        [Required]
        public string ImageUrl { get; set; } // Url của ảnh



    }
}
