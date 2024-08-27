using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HotelApp.Models.Hotel.RoomBooking;

namespace HotelApp.Models.Hotel
{ 
    // đại diện cho thông tin phòng của khách sạn
    public class Room
    {
        [Key]
        public int RoomId { get; set; } // mã phòng
        [Required]
        public int HotelId { get; set; } // mã định danh của khách sạn mà phòng thuộc về
        [Required]
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; } // liên kết với Hotel để phòng này thuộc Hotel nào
        [Required]
        public int RoomNumber { get; set; } // tên phòng, vd phòng 101, 102, 103, ...
        [Required]
        public string RoomType { get; set; } // dạng phòng, vd double, family, single, suite
        [Required]
        [DataType(DataType.Currency)] // attributes kiểu tiền tệ
        public decimal Price { get; set; } // giá tiền của phòng
        public enum StatusRoom
        {
            Available,
            Occupied,
            Maintenance
        } // Ví dụ: "Available", "Occupied", "Maintenance"
        public StatusRoom StatusRooms { get; set; }
        [Required]
        public int BedCount { get; set; } // Số lượng giường
        public ICollection<RoomImage> RoomImages { get; set; }// ảnh phòng
        public ICollection<RoomBooking> RoomBookings { get; set; } // Cách đặt phòng của phòng này
    }
}
