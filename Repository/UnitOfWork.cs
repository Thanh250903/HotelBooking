using HotelApp.Data;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
       private readonly ApplicationDBContext _dbContext;
       public IHotelRepository HotelRepository { get; set; }

       public UnitOfWork(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            HotelRepository = new HotelRepository(dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
