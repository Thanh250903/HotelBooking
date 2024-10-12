using HotelApp.Models.Hotel;
using System.Linq.Expressions;


namespace HotelApp.Repository.IRepository
{
	public interface IRoomRepository : IRepository<Room>
	{
        void Update(Room entity);
        Room GetById(int id);
        IEnumerable<Room> GetRoomsByHotelId(int hotelId);
        IEnumerable<Room> GetAll(Expression<Func<Room, bool>> filter = null);
    }
}