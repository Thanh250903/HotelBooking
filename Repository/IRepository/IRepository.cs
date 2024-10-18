using HotelApp.Models.Hotel;
using System.Linq.Expressions;

namespace HotelApp.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
        IEnumerable<T> GetAll(string? includeProperty = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperty = null);
        T FirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Delete(T entity);
        Room GetFirstOrDefault(Expression<Func<Room, bool>> filter, Room defaultValue = null, string includeProperties = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Hotel GetFirstOrDefault(Expression<Func<Hotel, bool>> filter, string? includeProperties = null);
        IEnumerable<T> Include(params Expression<Func<T, object>>[] includeProperties);
      


    }
}