using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models.Hotel.VM
{
    public class ConfirmBookingVM
    {
        public int RoomBookingId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string PhoneNumber {  get; set; }
        [Required]
        public string Email { get; set; }

    }
}
