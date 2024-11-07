using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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
            return _dbContext.Rooms.Include(room => room.Hotel)
                             .FirstOrDefault(room => room.RoomId == id); 
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
            return !_dbContext.Rooms.Any(r => r.HotelId == hotelId && r.RoomNumber == roomNumber); // not use
        }

        public async Task<Room> GetRoomById(int roomId)
        {
            return await _dbContext.Rooms.FindAsync(roomId);
        }

        public async Task AddRoom(Room room)
        {
           await _dbContext.Rooms.AddAsync(room);
        }

        public async Task<bool> IsRoomNumberUniqueAsync(int hotelId, int roomNumber)
        {
           return !await _dbContext.Rooms.AnyAsync(room => room.HotelId == hotelId && room.RoomNumber == roomNumber);
        }

        public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _dbContext.Rooms.Where(room => room.HotelId == hotelId).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _dbContext.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllRoomAsync()
        {
           return await _dbContext.Rooms.ToListAsync();
        }
    }
}

