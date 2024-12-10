using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApp.Repository
{
    public class BookingRepository : Repository<BookingRepository>, IBookingRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public BookingRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        // Take list of booking by UserId

        public IEnumerable<RoomBooking> GetBookingsByUserId(string userId)
        {
            return _dbContext.RoomBookings.Include(roombooing => roombooing.Room).Where(roombooking => roombooking.UserId == userId).ToList();
        }
        //Take list of booking by RoomId

        public IEnumerable<RoomBooking> GetBookingByRoomId (int roomId)
        {
            return _dbContext.RoomBookings.Where(r => r.RoomId == roomId).ToList();
        }

        public RoomBooking GetFirstOrDefault(Expression<Func<RoomBooking, bool>> filter, string includeProperties = null)
        {
            IQueryable<RoomBooking> query = _dbContext.RoomBookings;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }


        // Triển khai GetAll
        public IEnumerable<RoomBooking> GetAll(string? includeProperties = null)
        {
            IQueryable<RoomBooking> query = _dbContext.RoomBookings;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        // Triển khai Get
        public RoomBooking Get(Expression<Func<RoomBooking, bool>> filter, string? includeProperties = null)
        {
            IQueryable<RoomBooking> query = _dbContext.RoomBookings;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault(filter);
        }

        public void Add(RoomBooking entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(RoomBooking entity)
        {
            _dbContext.Remove(entity);
        }
        public void Update(RoomBooking booking)
        {
            _dbContext.RoomBookings.Update(booking);
        }

        public IEnumerable<RoomBooking> Include(params Expression<Func<RoomBooking, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public RoomBooking FirstOrDefault(Expression<Func<RoomBooking, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public RoomBooking Get(Expression<Func<RoomBooking, bool>> filter)
        {
            return _dbContext.RoomBookings.FirstOrDefault(filter);
        }

        public Room GetRoomFirstOrDefault(Expression<Func<Room, bool>> filter, Room defaultValue = null, string includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomBooking> GetRoomBookingByIdAsync(int id)
        {
            return await _dbContext.RoomBookings.FirstOrDefaultAsync(roomboking => roomboking.RoomBookingId == id);
        }
    }
}
