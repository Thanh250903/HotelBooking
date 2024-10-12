using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using System.Linq.Expressions;

namespace HotelApp.Repository
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public RoomRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Room entity)
        {
            _dbContext.Rooms.Update(entity);
        }
        
        public Room GetById(int id)
        {
            return _dbContext.Rooms.Find(id);
        }
        // querry room by id
        // lấy ds phòng của một khách sạn cụ thể
        public IEnumerable<Room> GetRoomsByHotelId(int HotelId) 
        {
            return _dbContext.Rooms.Where(r => r.HotelId == HotelId).ToList();
        }
        // xem ds tất cả các phòng
        public IEnumerable<Room> GetAll(Expression<Func<Room, bool>> filter = null) 
        {
            IQueryable<Room> query = _dbContext.Rooms;

            // if filter not null 
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }
        
        public bool IsRoomNumberUnique(int hotelId, int roomNumber)
        {
            return !_dbContext.Rooms.Any(r => r.HotelId == hotelId && r.RoomNumber == roomNumber);
        }

    }
}
