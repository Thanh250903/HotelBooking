namespace HotelApp.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHotelRepository HotelRepository { get; set; }
        IRoomRepository RoomRepository { get; set; }
        void Save();
    }
}
