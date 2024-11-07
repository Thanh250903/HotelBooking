﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Hotel
{
    // đại diện cho thông tin cơ bản của khách sạn
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; } // mã khách sạn
        [Required(ErrorMessage = "Hotel name can not be empty")]
        [StringLength(50)]
        public string HotelName { get; set; } // tên khách sạn
        [ValidateNever]
        public string Description { get; set; } // mô tả ksan
        [ValidateNever]
        public string City { get; set; } // ksan đó nằm ở vị trí nào
        [ValidateNever]
        public int NumberPhone { get; set; } // sđt của khách sạn
        [NotMapped] // kh lưu được ở trong CSDL và khi thêm khách sạn sẽ kh hiện trường này lên, nó sẽ lấy dữ liệu từ HotelReview
        public double AverangeRating
        {
            get
            {
                if (HotelReviews != null && HotelReviews.Any())
                {
                    return HotelReviews.Average(review => review.Rating);
                }
                return 0; // Giá trị mặc định khi không có đánh giá nào
            }
        } // trường này đùng để tính toán số lượng Review đánh giá về khách sạn, không lưu trữ trong Database.
        public double Lattitube { get; set; } // kinh độ
        [ValidateNever]
        public double Longitude { get; set; } // vĩ độ
        [ValidateNever]
        public string? ImageUrl { get; set; } // đường dẫn ảnh
        [ValidateNever]
        public ICollection<Room>? Rooms { get; set; } // tùy thuộc vào tình trạng phòng để quản lý phòng dễ dàng hơn, cho phép đặt phòng trực tuyến và chọn phòng như ý muốn
        public ICollection<RoomBooking>? RoomBookings { get; set; } // set với RoomBooking
        public ICollection<HotelReview>? HotelReviews { get; set; } // danh sách về các đánh giá của khách sạn
        public string OwnerId {  get; set; }
    }
}
