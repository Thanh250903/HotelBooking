using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HotelApp.Models.Hotel.VM
{
    public class RoomVM
    {
        // lấy tt khách sạn và danh sách phòng
        public Hotel Hotel { get; set; }
        [ValidateNever]
        public IEnumerable<Room> Rooms { get; set; }
        [ValidateNever]
        public Room Room { get; set; }
    }
}
