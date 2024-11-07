using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApp.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public HotelRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Hotel entity)
        {
            _dbContext.Hotels.Update(entity);
        }

        public Hotel GetById(int id)
        {
            return _dbContext.Hotels.Find(id);
        }

        public Hotel GetFirstOrDefault(Expression<Func<Hotel, bool>> filter, string includeProperties = null)
        {
            IQueryable<Hotel> query = _dbContext.Hotels;

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

        public async Task<IEnumerable<Hotel>> GetHotelByOwnerId(string ownerId)
        {
          return await _dbContext.Hotels.Where(o => o.OwnerId == ownerId).ToListAsync();
        }
        public async Task<bool> IsHotelOwnerAsync(int hotelId,  string ownerId)
        {
            return await _dbContext.Hotels.AnyAsync(h => h.HotelId == hotelId  && h.OwnerId == ownerId);
        }

        public async Task<Hotel> GetHotelById(int hotelId)
        {
            return await _dbContext.Hotels.FindAsync(hotelId);
        }
    }
 }
