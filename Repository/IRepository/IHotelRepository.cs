
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository.IRepository
{
	public interface IHotelRepository : IRepository<Hotel>
	{
		void Update(Hotel entity);
	}
}