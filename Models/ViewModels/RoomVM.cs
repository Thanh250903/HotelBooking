using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HotelApp.Models.ViewModel
{
    public class RoomVM
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        [ValidateNever]
        public ICollection<Room> Rooms {  get; set; }
    }
}
