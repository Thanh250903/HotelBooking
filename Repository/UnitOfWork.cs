﻿using HotelApp.Data;
using HotelApp.Repository.IRepository;

namespace HotelApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
       private readonly ApplicationDBContext _dbContext;
       public IHotelRepository HotelRepository { get; set; }
       public IRoomRepository RoomRepository { get; set; }
       public IBookingRepository BookingRepository { get; set; }
       public IPaymentRepository PaymentRepository { get; set; }

       public UnitOfWork(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            HotelRepository = new HotelRepository(dbContext);
            RoomRepository = new RoomRepository(dbContext);
            BookingRepository = new BookingRepository(dbContext);
            PaymentRepository = new PaymentRepository(dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }

       public async Task SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
           _dbContext.Dispose();
        }
    }
}
