using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HotelApp.Models.Hotel.VM
{
    public class RoomVM
    {
        // lấy tt khách sạn và danh sách phòng
        public int HotelId { get; set; }
        [ValidateNever]
        public string HotelName { get; set; }
        [ValidateNever]
        public Hotel Hotel { get; set; }
        [ValidateNever]
        public int RoomId { get; set; }
        [ValidateNever]
        public Room Room { get; set; }
        [ValidateNever]
        public int RoomNumber { get; set; }
        [ValidateNever]
        public string RoomType { get; set; }
        [ValidateNever]
        public decimal Price { get; set; }
        [ValidateNever]
        public Room.StatusRoom StatusRoom {  get; set; }
        [ValidateNever]
        public int BedCount {  get; set; }
       
        private string _roomImgUrl;
        [ValidateNever]
        public string RoomImgUrl
        {
            get { return _roomImgUrl; }
            set { _roomImgUrl = value; }
        }
        public bool IsOwner { get; set; } // confrim Owner belong this hotel or not
    }
}
