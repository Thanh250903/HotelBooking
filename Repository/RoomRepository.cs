using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

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

        public IEnumerable<Room> GetRoomsByHotelId(int HotelId) 
        {
            return _dbContext.Rooms.Where(r => r.HotelId == HotelId).ToList();
        }
        
    }
}
