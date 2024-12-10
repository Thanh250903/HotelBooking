namespace HotelApp.Models.Hotel.VM
{
    public class HotelDetailsVM
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsOwner { get; set; }
        public Hotel Hotel { get; set; }
        public string Comment {  get; set; }

        // List of room
        public ICollection<RoomVM> Rooms { get; set; }

        // List of comment
        public IEnumerable<HotelReview> HotelReviews { get; set; }

        // New commemt
        public HotelReview NewHotelReview { get; set; }
        public int TotalReviews {  get; set; }
    }

}
