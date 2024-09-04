using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository
{
	public class HotelRepository : Repository<Hotel>, IHotelRepository
	{
		private readonly ApplicationDBContext _dbContext;
		public HotelRepository(ApplicationDBContext dBContext) : base(dBContext)
		{
			_dbContext = dBContext;
		}
		public void Update(Hotel entity)
		{
			_dbContext.Hotels.Update(entity);
		}
	}
}