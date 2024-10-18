using HotelApp.Models.Hotel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models.Payment
{
    public class PersonDetail
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; } // mã định danh của phòng được đặt
        [ValidateNever]
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // liên kết với đối tượng Room, xem phòng nào đã được đặt
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string NumberPhone { get; set; }
        [Required]
        public string Country { get; set; }

    }
}
