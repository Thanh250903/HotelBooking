
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository.IRepository
{
	public interface IHotelRepository : IRepository<Hotel>
	{
        Hotel GetById(int id);
        void Update(Hotel entity);
		Task<IEnumerable<Hotel>> GetHotelByOwnerId(string ownerId);
		Task<bool> IsHotelOwnerAsync(int hotelId, string ownerId);
		Task<Hotel> GetHotelById(int hotelId);
	}
}