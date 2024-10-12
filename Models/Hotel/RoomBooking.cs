﻿using HotelApp.Models.Others;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Hotel
{
    // đại diện cho thông tin đặt phòng của khách sạn
    public class RoomBooking
    {
        [Key]
        public int RoomBookingId { get; set; } // mã định danh đặt phòng
        [Required]
        public int RoomId { get; set; } // mã định danh của phòng được đặt
        [Required]
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // liên kết với đối tượng Room, xem phòng nào đã được đặt
        [Required]
        public string UserId { get; set; } // mã định danh của người đặt phòng
        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } // liên kết với đối tượng User, xem người nào đã đặt phòng
        [Required]
        [Display(Name = "Choose checkin date")]
        public DateTime CheckInDate { get; set; } // ngày đến
        [Required]
        [Display(Name = "Choose checkout date")]
        public DateTime CheckOutDate { get; set; } // ngày đi
        [Required]
        [DataType(DataType.Currency)] // attributes kiểu tiền tệ
        public decimal TotalPrice { get; set; }
        public int? PaymentId { get; set; } // Null nếu kh có thanh toán
        [ForeignKey("PaymentId")]
        // Thêm navigation property từ RoomBooking đến Payment
        public Payment? Payment { get; set; } // Tham chiếu tới Payment nếu có thanh toán

    }
}
