using HotelApp.Models.Hotel;
using System.Linq.Expressions;


namespace HotelApp.Repository.IRepository
{
	public interface IRoomRepository : IRepository<Room>
	{
        void Update(Room entity);
        Room GetById(int id); // not use
        IEnumerable<Room> GetRoomsByHotelId(int hotelId); // not use
        IEnumerable<Room> GetAll(Expression<Func<Room, bool>> filter = null); // not use
        bool IsRoomNumberUnique(int hotelId, int roomNumber); // not use

        Task<IEnumerable<Room>> GetAllRoomAsync();
        Task AddRoom(Room room);
        Task<bool> IsRoomNumberUniqueAsync(int hotelId, int roomNumber);
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
        Task SaveAsync();
        Task<Room> GetRoomByIdAsync(int id);

    }
}