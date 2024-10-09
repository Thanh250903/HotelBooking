using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

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
    }
}
