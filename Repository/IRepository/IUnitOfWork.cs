namespace HotelApp.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHotelRepository HotelRepository { get; set; }
        void Save();
    }
}
