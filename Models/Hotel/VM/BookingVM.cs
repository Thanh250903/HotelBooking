using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models.Hotel.VM
{
    public class BookingVM
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        //public string UserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
