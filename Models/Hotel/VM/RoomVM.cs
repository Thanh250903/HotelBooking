using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HotelApp.Models.Hotel.VM
{
    public class RoomVM
    {
        // ID của khách sạn và thông tin
        [Required(ErrorMessage = "Hotel ID is required.")]
        public int HotelId { get; set; }

        [ValidateNever]
        public string HotelName { get; set; }

        [ValidateNever]
        public Hotel Hotel { get; set; }

        // ID của phòng
        [Required(ErrorMessage = "Room ID is required.")]
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; }

        // Số phòng
        [Required(ErrorMessage = "Room Number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Room Number must be greater than 0.")]
        public int RoomNumber { get; set; }

        // Loại phòng
        [Required(ErrorMessage = "Room Type is required.")]
        [StringLength(50, ErrorMessage = "Room Type must not exceed 50 characters.")]
        public string RoomType { get; set; }
        // Giá phòng
        [Required(ErrorMessage = "Price is required.")]
        public int Price { get; set; }

        // Trạng thái phòng
        [Required(ErrorMessage = "Room status is required.")]
        public Room.StatusRoom StatusRoom { get; set; }

        // Số giường
        [Required(ErrorMessage = "Bed Count is required.")]
        [Range(1, 10, ErrorMessage = "Bed Count must be between 1 and 10.")]
        
        public int BedCount { get; set; }

        // URL hình ảnh
        [ValidateNever]
        public string RoomImgUrl { get; set; }

        // Xác nhận chủ sở hữu
        [ValidateNever]
        public bool IsOwner { get; set; } // Confirm Owner belongs to this hotel or not

    }
}
