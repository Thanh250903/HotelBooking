using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Repository.Repository
{
    public class HotelReviewRepository : Repository<HotelReview>, IHotelReviewRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public HotelReviewRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HotelReview> GetByIdAsync(int hotelId)
        {
            return await _dbContext.HotelReviews.Include(review => review.Hotel).FirstOrDefaultAsync(review => review.ReviewId == hotelId);
        }

        public async Task<IEnumerable<HotelReview>> GetReviewsByHotelIdAsync(int hotelId)
        {
           return await _dbContext.HotelReviews.Where(r => r.HotelId == hotelId).Include(r => r.User).ToListAsync();
        }

        public async Task<IEnumerable<HotelReview>> GetReviewsByUserIdAsync(string userId)
        {
            return await _dbContext.HotelReviews.Where(r => r.UserId == userId).ToListAsync();
        }
    }
}
