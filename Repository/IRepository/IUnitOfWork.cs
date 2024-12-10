namespace HotelApp.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IHotelRepository HotelRepository { get; set; }
        IRoomRepository RoomRepository { get; set; }
        IBookingRepository BookingRepository { get; set; }
        IPaymentRepository PaymentRepository { get; set; }  
        IHotelReviewRepository HotelReviewRepository { get; set; }
        
        void Save();
        Task SaveAsync();
    }
}
