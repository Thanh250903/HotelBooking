using HotelApp.Data;
using HotelApp.Models.Hotel;
using HotelApp.Models.Others;
using HotelApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public PaymentRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Payment entity)
        {
            _dbContext.Payments.Update(entity);
        }
        
    }
}
