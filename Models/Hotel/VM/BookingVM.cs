using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HotelApp.Models.Hotel.VM
{
    public class BookingVM
    {
        public string UserId { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        public int TotalPrice { get; set; }
        public int Price { get; set; }
    }
}
