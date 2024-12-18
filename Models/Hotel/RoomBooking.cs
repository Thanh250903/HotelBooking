﻿using HotelApp.Models.Others;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace HotelApp.Models.Hotel
{
    // đại diện cho thông tin đặt phòng của khách sạn
    public class RoomBooking
    {
        [Key]
        public int RoomBookingId { get; set; } // mã định danh đặt phòng kh sạn
        [ValidateNever]
        public int RoomId { get; set; } // mã định danh của phòng được đặt
        [ValidateNever]
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // liên kết với đối tượng Room, xem phòng nào đã được đặt
        public string? UserId { get; set; } // mã định danh của người đặt phòng
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; } // liên kết với đối tượng User, xem người nào đã đặt phòng
        [ValidateNever]
        public DateTime? BookingDate { get; set; }
        [ValidateNever]
        [Display(Name = "Choose checkin date")]
        public DateTime CheckInDate { get; set; } // ngày đến
        [ValidateNever]
        [Display(Name = "Choose checkout date")]
        public DateTime CheckOutDate { get; set; } // ngày đi
        [DataType(DataType.Currency)] // attributes kiểu tiền tệ
        [ValidateNever]
        public int TotalPrice { get; set; }
        [ValidateNever]
        public int PricePerNight { get; set; }
        public bool IsPaid { get; set; } = false;
       
    }
}
