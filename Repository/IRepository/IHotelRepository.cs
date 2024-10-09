
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository.IRepository
{
	public interface IHotelRepository : IRepository<Hotel>
	{
        Hotel GetById(int id);
        void Update(Hotel entity);

	}
}