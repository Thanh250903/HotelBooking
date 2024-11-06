using HotelApp.Models.Hotel;

namespace HotelApp.Repository.IRepository
{
    public interface IBookingRepository: IRepository<RoomBooking>
    {
        IEnumerable<RoomBooking> GetBookingsByUserId(string userId);
        IEnumerable<RoomBooking> GetBookingByRoomId(int roomId);    

    }
}
