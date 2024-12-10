
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository.IRepository
{
	public interface IHotelReviewRepository : IRepository<HotelReview>
	{
        Task<IEnumerable<HotelReview>> GetReviewsByHotelIdAsync(int hotelId);
        Task<IEnumerable<HotelReview>> GetReviewsByUserIdAsync(string userId);
        Task<HotelReview> GetByIdAsync(int hotelId);
    }
}