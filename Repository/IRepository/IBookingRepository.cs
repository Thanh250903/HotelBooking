using HotelApp.Models.Hotel;
using System.Linq.Expressions;

namespace HotelApp.Repository.IRepository
{
    public interface IBookingRepository: IRepository<RoomBooking>
    {
        IEnumerable<RoomBooking> GetBookingsByUserId(string userId);
        IEnumerable<RoomBooking> GetBookingByRoomId(int roomId);
        void Add(RoomBooking booking);
        RoomBooking Get(Expression<Func<RoomBooking, bool>> filter);
        void Update(RoomBooking booking);
        Task<RoomBooking> GetRoomBookingByIdAsync(int id);

    }
}
