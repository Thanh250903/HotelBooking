
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository.IRepository
{
	public interface IHotelRepository : IRepository<Hotel>
	{
        Hotel GetById(int id); // not use
        void Update(Hotel entity);
		Task<IEnumerable<Hotel>> GetHotelByOwnerId(string ownerId);
		Task<Hotel> GetHotelByIdAsync(int hotelId);
		Task<IEnumerable<Hotel>> GetAllHotelAsync();
        Task<bool> IsHotelOwnerAsync(int id, string ownerId);

    }
}